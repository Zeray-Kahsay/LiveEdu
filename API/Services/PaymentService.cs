
using API.Data;
using API.DTOs.Payments;
using API.Entities.Carts;
using API.Helpers;
using API.Interfaces.Orders;
using API.Interfaces.Payments;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;

namespace API.Services;

public interface IPaymentService
{
    Task<Result<PaymentIntentResponseDto>> CreatePaymentIntentAsync(CreatePaymentIntentDto dto);
    Task<Result> HandleWebhookAsync(string json, string signatureHeader);
}

public class PaymentService : IPaymentService
{
    private readonly StripeSettings _settings;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly AppDbContext _context;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(
        IOptions<StripeSettings> options,
        IPaymentRepository paymentRepository,
        IOrderRepository orderRepository,
        AppDbContext context,
        ILogger<PaymentService> logger)
    {
        _settings = options.Value;
        StripeConfiguration.ApiKey = _settings.SecretKey;
        _paymentRepository = paymentRepository;
        _orderRepository = orderRepository;
        _context = context;
        _logger = logger;

        Console.WriteLine($"Stripe Webhook Key {_settings.WebhookSecret} - {_settings.PublishableKey} - {_settings.SecretKey}");
    }

    public async Task<Result<PaymentIntentResponseDto>> CreatePaymentIntentAsync(CreatePaymentIntentDto dto)
    {
        // load order 
        var order = await _orderRepository.GetOrderByIdAsync(dto.OrderId);
        if (order is null) return Result<PaymentIntentResponseDto>.Failure("Order not found");
        if (order.IsPaid) return Result<PaymentIntentResponseDto>.Failure("Order is already paid");

        // calculate amount in cents
        var total = order.Total;
        if (total <= 0m)
        {
            total = order.Items.Sum(i => i.Price * i.Quantity);
        }

        if (total <= 0m)
        {
            return Result<PaymentIntentResponseDto>.Failure("Order total must be greater than zero");
        }

        var amount = (long)Math.Round(total * 100m);

        var options = new PaymentIntentCreateOptions
        {
            Amount = amount,
            Currency = dto.Currency ?? "usd",
            PaymentMethodTypes = new List<string> { "card" },
            Metadata = new Dictionary<string, string>
            {
                {"orderId", order.OrderId.ToString() },
                {"userId", order.UserId?.ToString() ?? "" }
            }
        };

        var service = new PaymentIntentService();
        var pi = await service.CreateAsync(options);

        // Create Payment entity and link to order
        var payment = new Payment
        {
            PaymentIntentId = pi.Id,
            Provider = "Stripe",
            Status = PaymentStatus.Pending,
            OrderId = order.OrderId,
            CreatedAt = DateTime.UtcNow,
        };

        await _paymentRepository.AddPaymentAsync(payment);
        await _paymentRepository.SaveChangesAsync();

        var response = new PaymentIntentResponseDto
        {
            ClientSecret = pi.ClientSecret ?? string.Empty,
            PaymentIntentId = pi.Id,
            PublishableKey = _settings.PublishableKey
        };

        return Result<PaymentIntentResponseDto>.Success(response);




    }

    public async Task<Result> HandleWebhookAsync(string json, string signatureHeader)
    {
        try
        {
            Event stripeEvent;
            if (!string.IsNullOrEmpty(_settings.WebhookSecret))
            {
                stripeEvent = EventUtility.ConstructEvent(json, signatureHeader, _settings.WebhookSecret);
            }
            else
            {
                stripeEvent = EventUtility.ParseEvent(json);
            }

            switch (stripeEvent.Type)
            {
                case StripeEventTypes.PaymentIntentSucceeded:
                    await HandlePaymentIntentSucceededAsync(stripeEvent);
                    break;
                case StripeEventTypes.PaymentIntentFailed:
                    await HandlePaymentIntentFailedAsync(stripeEvent);
                    break;
                default:
                    _logger.LogInformation("Unhandled Stripe event type: {EventType}", stripeEvent.Type);
                    break;
            }

            return Result.Success();
        }
        catch (StripeException sEX)
        {
            _logger.LogError(sEX, "Stripe webhook error");
            return Result.Failure($"Stripe webhook error: {sEX.Message}");

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Webhook processing error");
            return Result.Failure($"Webhook processing error: {ex.Message}");
        }
    }


    private async Task HandlePaymentIntentSucceededAsync(Event stripeEvent)
    {
        if (stripeEvent.Data.Object is not PaymentIntent pi)
        {
            _logger.LogError("Invalid payment intent object in webhook event");
            return;
        }

        var payment = await _paymentRepository.GetPaymentByPaymentIntentIdAsync(pi.Id);
        if (payment is null)
        {
            _logger.LogError("Payment not found for PaymentIntentId: {PaymentIntentId}", pi.Id);
            return;
        }

        // update payment and order status
        payment.Status = PaymentStatus.Succeeded;
        payment.UpdatedAt = DateTime.UtcNow;
        await _paymentRepository.SaveChangesAsync();

        var order = await _orderRepository.GetOrderByIdAsync(payment.OrderId);
        if (order is not null)
        {
            order.IsPaid = true;
            order.PaidAt = DateTime.UtcNow;
            order.Payment!.Status = PaymentStatus.Succeeded; // TODO: C#14 null safety
            order.PaymentId = payment.PaymentId;
            await _orderRepository.UpdateOrder(order);
            await _orderRepository.SaveChangesAsync();

            // enroll user if userId present
            if (order.UserId.HasValue)
            {
                var userId = order.UserId.Value;
                foreach (var item in order.Items)
                {
                    var exists = await _context.Enrollments.AnyAsync(e => e.CourseId == item.CourseId && e.StudentId == userId);
                    if (!exists)
                    {
                        _context.Enrollments.Add(new Entities.Courses.Enrollment
                        {
                            CourseId = item.CourseId,
                            StudentId = userId,
                            EnrolledAt = DateTime.UtcNow,
                            Status = Entities.Courses.EnrollmentStatus.Enrolled

                        });
                    }

                }
                await _context.SaveChangesAsync();
            }

        }
        _logger.LogInformation("PaymentIntent {PaymentIntentId} succeeded and order {OrderId} marked as paid", pi.Id, payment.OrderId);
    }

    private async Task HandlePaymentIntentFailedAsync(Event stripeEvent)
    {
        if (stripeEvent.Data.Object is not PaymentIntent pi)
        {
            _logger.LogWarning("Invalid payment intent object in webhook");
            return;
        }

        var payment = await _paymentRepository.GetPaymentByPaymentIntentIdAsync(pi.Id);
        if (payment is null)
        {
            _logger.LogWarning("Payment not found for PaymentIntent {Id}", pi.Id);
            return;
        }

        payment.Status = PaymentStatus.Failed;
        payment.UpdatedAt = DateTime.UtcNow;
        await _paymentRepository.SaveChangesAsync();

        var order = await _orderRepository.GetOrderByIdAsync(payment.OrderId);
        if (order is not null)
        {
            order.Payment!.Status = PaymentStatus.Failed; // TODO: C#14 null safety
            await _orderRepository.UpdateOrder(order);
            await _orderRepository.SaveChangesAsync();
        }

        _logger.LogInformation("Handled payment intent failed for {PaymentIntentId}", pi.Id);
    }
}


