using API.Data;
using API.DTOs.Carts;
using API.Entities.Carts;
using API.Entities.Courses;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace API.Controllers;

public class PaymentsController(
    StripePaymentService paymentService,
    AppDbContext context,
    ILogger<PaymentsController> logger,
    IConfiguration config) : BaseController
{
    [HttpPost("create-payment-intent")]
    public async Task<ActionResult<PaymentIntentResponseDto>> CreatePaymentIntent([FromBody] Cart dto)
    {

        var cart = await context.Carts
                    .Include(c => c.Items)
                    .ThenInclude(i => i.Course)
                    .FirstOrDefaultAsync(c => c.CartId == dto.CartId);

        if (cart is null)
            return BadRequest("Problem with the cart");

        var intent = await paymentService.CreateOrUpdatePaymentIntent(cart);

        if (intent is null)
            return BadRequest("Problem creating payment intent");

        cart.PaymentIntentId ??= intent.Id;
        cart.ClientSecretId ??= intent.ClientSecret;

        if (context.ChangeTracker.HasChanges())
        {
            if (await context.SaveChangesAsync() <= 0)
                return BadRequest("Problem updating cart with intent");
        }

        var publishableKey = config["StripeSettings:PublishableKey"];


        return new PaymentIntentResponseDto
        {
            ClientSecret = intent.ClientSecret,
            PublishableKey = publishableKey!
        };


    }


    [HttpPost("webhook")]
    public async Task<IActionResult> Webhook()
    {
        var json = await new StreamReader(Request.Body).ReadToEndAsync();

        try
        {
            var stripeEvent = ConstructStripeEvent(json);

            if (stripeEvent.Data.Object is not PaymentIntent intent)
            {
                logger.LogWarning("Invalid event data: no PaymentIntent found");
                return BadRequest("Invalid event data");
            }

            switch (intent.Status)
            {
                case "succeeded":
                    await HandlePaymentIntentSucceeded(intent);
                    break;
                case "requires_payment_method":
                case "canceled":
                case "payment_failed":
                    await HandlePaymentIntentFailed(intent);
                    break;
                default:
                    logger.LogInformation("Unhandled Stripe status: {Status}", intent.Status);
                    break;
            }

            return Ok();
        }
        catch (StripeException ex)
        {
            logger.LogError(ex, "Stripe webhook signature verification failed");
            return BadRequest("Invalid Stripe signature");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected webhook error");
            return StatusCode(500, "Webhook error");
        }
    }



    // [HttpPost("webhook")]
    // public async Task<IActionResult> Webhook()
    // {

    //     var json = await new StreamReader(Request.Body).ReadToEndAsync();

    //     try
    //     {
    //         var stripeEvent = ConstructStripeEvent(json);

    //         if (stripeEvent.Data.Object is not PaymentIntent intent)
    //         {
    //             return BadRequest("Invalid event data");
    //         }

    //         if (intent.Status == "succeeded") await HandlePaymentIntentSucceeded(intent);
    //         else await HandlePaymentIntentFailed(intent);

    //         return Ok();
    //     }
    //     catch (StripeException ex)
    //     {
    //         logger.LogError(ex, "Stripe error");
    //         return StatusCode(StatusCodes.Status500InternalServerError, "Webhook error");
    //     }
    //     catch (Exception ex)
    //     {
    //         logger.LogError(ex, "An Uexpected error has occurred");
    //         return StatusCode(StatusCodes.Status500InternalServerError, "Unexpected error");
    //     }


    // }



    private Event ConstructStripeEvent(string json)
    {
        try
        {
            return EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], config["StripeSettings:WhSecret"]);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to construct stripe event");
            throw new StripeException("Invalid signature");
        }
    }

    private async Task HandlePaymentIntentSucceeded(PaymentIntent intent)
    {
        var cartId = intent.Metadata.GetValueOrDefault("cartId");
        var userId = intent.Metadata.GetValueOrDefault("userId");

        if (string.IsNullOrEmpty(cartId))
        {
            logger.LogWarning("Payment succeeded but no cartId in metadata");
            return;
        }

        var cart = await context.Carts
            .Include(c => c.Items)
            .FirstOrDefaultAsync(c => c.CartId == cartId);

        if (cart is null)
        {
            logger.LogWarning("Cart not found for PaymentIntent {Id}", intent.Id);
            return;
        }

        if (cart.UserId is null)
        {
            logger.LogWarning("Guest checkout - skipping enrollment");
            return;
        }

        // Enroll user in all courses
        foreach (var item in cart.Items)
        {
            var exists = await context.Enrollments
                .AnyAsync(e => e.CourseId == item.CourseId && e.StudentId == cart.UserId);

            if (!exists)
            {
                context.Enrollments.Add(new Enrollment
                {
                    CourseId = item.CourseId,
                    StudentId = cart.UserId.Value,
                    EnrolledAt = DateTime.UtcNow,
                    Status = EnrollmentStatus.Enrolled
                });
            }
        }

        // Clear cart after success
        context.CartItems.RemoveRange(cart.Items);
        context.Carts.Remove(cart);

        await context.SaveChangesAsync();

        logger.LogInformation(
            "PaymentIntent {IntentId} succeeded. User {UserId} enrolled and cart {CartId} cleared.",
            intent.Id, cart.UserId, cart.CartId
        );
    }




    private async Task HandlePaymentIntentFailed(PaymentIntent intent)
    {
        var cartId = intent.Metadata.GetValueOrDefault("cartId");
        logger.LogWarning("PaymentIntent {Id} failed for cart {CartId}", intent.Id, cartId);
        await Task.CompletedTask;
    }
}
