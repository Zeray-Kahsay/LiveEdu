
using System.Collections.Generic;
using API.Data;
using API.DTOs.cart;
using API.Entities.CourseCart;
using API.Helpers;
using API.Interfaces.CourseCart;
using API.Services;
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



    public Task<Result> HandleWebhookAsync(string json, string signHeader)
    {
        throw new NotImplementedException();
    }
}
