
using API.Data;
using API.DTOs.cart;
using API.Entities;
using API.Entities.CourseCart;
using API.Helpers;
using API.Interfaces.CourseCart;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;

namespace API.Repositories.CourseCart;

public class PaymentService : IPaymentService
{
    private readonly StripeSettings _settings;
    private readonly IOrderRepository _orderRepository;
    private readonly AppDbContext _context;
    private readonly ILogger<PaymentService> _logger;

    public PaymentService(IOptions<StripeSettings> options, IOrderRepository orderRepository, AppDbContext context, ILogger<PaymentService> logger)
    {
        _settings = options.Value;
        StripeConfiguration.ApiKey = _settings.SecretKey;
        _orderRepository = orderRepository;
        _context = context;
        _logger = logger;
    }
    public async Task<Result<PaymentIntentResponseDto>> CreatePaymentIntentAsync(CreatePaymentDto dto)
    {
        if (dto.Items.Count == 0)
            return Result<PaymentIntentResponseDto>.Failure("Cart is empty");

        // compute total in smallest currency unit(cents)
        var totalDecimal = dto.Items.Sum(i => i.Price * i.Quantity);
        var amount = (long)(totalDecimal * 100m);

        var options = new PaymentIntentCreateOptions
        {
            Amount = amount,
            Currency = dto.Currency,
            PaymentMethodTypes = ["card"],
            Metadata = new Dictionary<string, string>
            {
                {"userId", dto.UserId?.ToString() ?? ""}
            }
        };

        var service = new PaymentIntentService();
        var pi = await service.CreateAsync(options);

        // create order record - pending
        var order = new Order
        {
            UserId = dto.UserId,
            Total = totalDecimal,
            PaymentIntentId = pi.Id,
            IsPaid = false,
            Items = dto.Items.Select(it => new OrderItem
            {
                CourseId = it.CourseId,
                CourseTitle = it.Title,
                Price = it.Price,
                Quantity = it.Quantity
            }).ToList()
        };

        await _orderRepository.AddOrderAsync(order);
        await _orderRepository.SaveChangesAsync();

        var response = new PaymentIntentResponseDto
        {
            ClientSecret = pi.ClientSecret ?? string.Empty,
            PaymentIntentId = pi.Id,
            PublishableKey = _settings.PublishableKey
        };


        return Result<PaymentIntentResponseDto>.Success(response);

    }

    public async Task<Result> HandleWebhookAsync(string json, string signHeader)
    {
        try
        {
            Event stripeEvent;

            // Verify the webhook signature
            if (!string.IsNullOrEmpty(_settings.WebhookSecret))
            {
                stripeEvent = EventUtility.ConstructEvent(json, signHeader, _settings.WebhookSecret);

            }
            else
            {
                stripeEvent = EventUtility.ParseEvent(json);
            }

            // Handle events
            switch (stripeEvent.Type)
            {
                case StripeEventTypes.PaymentIntentSucceeded:
                    await HandlePaymentIntentSucceededAsync(stripeEvent);
                    break;
                case StripeEventTypes.PaymentIntentFailed:
                    await HandlePaymenetFailedAsync(stripeEvent);
                    break;
                case StripeEventTypes.CheckoutSessionCompleted:
                    HandleCheckoutSessionCompleted(stripeEvent);
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

    // Payment intent succeeded
    private async Task HandlePaymentIntentSucceededAsync(Event stripeEvent)
    {
        if (stripeEvent.Data.Object is not PaymentIntent pi)
        {
            _logger.LogWarning("Invalid payment intent object");
            return;
        }

        var order = await _orderRepository.GetByPaymentIntentIdAsync(pi.Id);
        if (order is null)
        {
            _logger.LogWarning("Order not found for paymentIntent {PaymentIntentId}", pi.Id);
            return;
        }

        order.IsPaid = true;
        order.PaymentStatus = "Succeeded";
        order.PaidAt = DateTime.UtcNow;

        _orderRepository.UpdateOrder(order);
        await _orderRepository.SaveChangesAsync();

        if (order.UserId.HasValue)
        {
            var userId = order.UserId.Value;
            foreach (var item in order.Items)
            {
                bool alreadyEnrolled = await _context.Enrollments.AnyAsync(e =>
                      e.CourseId == item.CourseId && e.StudentId == userId);

                if (!alreadyEnrolled)
                {
                    _context.Enrollments.Add(new Enrollment
                    {
                        CourseId = item.CourseId,
                        StudentId = userId,
                        EnrolledAt = DateTime.UtcNow,
                        Status = EnrollmentStatus.Enrolled
                    });
                }
            }

            await _context.SaveChangesAsync();
        }

        _logger.LogInformation("Payment succeeded and order {orderId} updated", order.OrderId);

    }

    // Payment Intent Failed 
    private async Task HandlePaymenetFailedAsync(Event stripeEvent)
    {
        if (stripeEvent.Data.Object is not PaymentIntent pi)
        {
            _logger.LogWarning("Invalid payment intent object");
            return;
        }

        var order = await _orderRepository.GetByPaymentIntentIdAsync(pi.Id);
        if (order is null)
        {
            _logger.LogWarning("Order not found for PaymentIntent {PaymentIntentId}", pi.Id);
        }


        if (order is not null)
        {
            order.PaymentStatus = "Failed";
            _orderRepository.UpdateOrder(order);

        }

        await _orderRepository.SaveChangesAsync();

        _logger.LogWarning("Payment failed for PaymentIntent {PaymentIntentId}", pi.Id);

    }

    // Payment Intent completed 
    private void HandleCheckoutSessionCompleted(Event stripeEvent)
    {
        if (stripeEvent.Data.Object is not Session session)
        {
            _logger.LogWarning("Invalid checkout session object");
            return;
        }

        _logger.LogWarning("Checkout session completed: {SessionId}", session.SessionId);
    }
}
