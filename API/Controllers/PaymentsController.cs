using API.DTOs.Payments;
using API.Helpers;
using API.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class PaymentsController(IPaymentService paymentService, ILogger<PaymentsController> logger) : BaseController
{
    [HttpPost("create-payment-intent")]
    public async Task<IActionResult> CreatePaymentIntent([FromBody] CreatePaymentIntentDto dto)
    {
        var result = await paymentService.CreatePaymentIntentAsync(dto);
        if (!result.IsSuccess)
        {
            logger.LogWarning("CreatePaymentIntent failed: {Errors}", string.Join(',', result.Errors ?? []));
            return BadRequest(new ApiErrorDto { Status = 400, Message = "Create payment intent failed", Errors = result.Errors });
        }

        return Ok(result.Value);
    }

    // Stripe webhook (no auth)
    [HttpPost("webhook")]
    public async Task<IActionResult> Webhook()
    {
        var body = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var signature = Request.Headers["Stripe-Signature"].FirstOrDefault() ?? string.Empty;

        logger.LogInformation("Stripe webhook received");
        var result = await paymentService.HandleWebhookAsync(body, signature);
        if (!result.IsSuccess)
        {
            logger.LogWarning("Webhook processing failed: {Errors}", string.Join(",", result.Errors ?? []));
            return BadRequest(new ApiErrorDto { Status = 400, Message = "Webhook processing failed", Errors = result.Errors });
        }

        return Ok();
    }


}
