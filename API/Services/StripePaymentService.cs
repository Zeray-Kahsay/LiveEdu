using API.Entities.Carts;
using Stripe;

namespace API.Services;

public class StripePaymentService
{
    private readonly IConfiguration _config;
    private readonly ILogger<StripePaymentService> _logger;

    public StripePaymentService(IConfiguration config, ILogger<StripePaymentService> logger)
    {
        _config = config;
        _logger = logger;
        StripeConfiguration.ApiKey = _config["StripeSettings:SecretKey"];
    }

    public async Task<PaymentIntent?> CreateOrUpdatePaymentIntent(Cart cart)
    {
        if (cart.Items == null || cart.Items.Count == 0)
        {
            _logger.LogWarning("Attempted to create PaymentIntent for an empty cart {CartId}", cart.CartId);
            return null;
        }

        var service = new PaymentIntentService();

        // Stripe expects amount in cents
        var total = cart.Items.Sum(i => i.Quantity * i.Price);
        var amount = (long)Math.Round(total * 100);

        try
        {
            PaymentIntent intent;

            if (string.IsNullOrEmpty(cart.PaymentIntentId))
            {
                // Create new PaymentIntent
                var createOptions = new PaymentIntentCreateOptions
                {
                    Amount = amount,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" },
                    Metadata = new Dictionary<string, string>
                    {
                        { "cartId", cart.CartId },
                        { "userId", cart.UserId?.ToString() ?? "guest" }
                    }
                };

                intent = await service.CreateAsync(createOptions);
                _logger.LogInformation("Created new PaymentIntent {Id} for cart {CartId}", intent.Id, cart.CartId);
            }
            else
            {
                // Update existing intent amount
                var updateOptions = new PaymentIntentUpdateOptions { Amount = amount };
                intent = await service.UpdateAsync(cart.PaymentIntentId, updateOptions);
                _logger.LogInformation("Updated PaymentIntent {Id} for cart {CartId}", intent.Id, cart.CartId);
            }

            return intent;
        }
        catch (StripeException ex)
        {
            _logger.LogError(ex, "Stripe error creating/updating PaymentIntent for cart {CartId}", cart.CartId);
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error creating/updating PaymentIntent for cart {CartId}", cart.CartId);
            throw;
        }
    }
}



// using API.Entities.Carts;
// using Stripe;

// namespace API.Services;

// public class StripePaymentService(IConfiguration config)
// {
//     public async Task<PaymentIntent> CreateOrUpdatePaymentIntent(Cart cart)
//     {
//         StripeConfiguration.ApiKey = config["StripeSettings:SecretKey"];

//         var service = new PaymentIntentService();

//         var intent = new PaymentIntent();

//         var amount = (long)(cart.Items.Sum(x => x.Quantity * x.Price) * 100);

//         if (string.IsNullOrEmpty(cart.PaymentIntentId))
//         {
//             var options = new PaymentIntentCreateOptions
//             {
//                 Amount = amount,
//                 Currency = "usd",
//                 PaymentMethodTypes = ["card"],
//                 Metadata = new Dictionary<string, string>
//                 {
//                     {"carrId", cart.CartId},
//                     {"userId", cart.UserId?.ToString() ?? "guest"}

//                 }

//             };
//             intent = await service.CreateAsync(options);
//         }
//         else
//         {
//             var options = new PaymentIntentUpdateOptions
//             {
//                 Amount = amount
//             };
//             await service.UpdateAsync(cart.PaymentIntentId, options);
//         }

//         return intent;
//     }
// }
