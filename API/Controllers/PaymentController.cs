using API.DTOs.cart;
using API.Helpers;
using API.Interfaces.CourseCart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger) : BaseController
{

    [HttpPost("create-payment-intent")]
    public async Task<IActionResult> CreatePaymentIntent([FromBody] CreatePaymentDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(new ApiErrorDto
            {
                Status = 400,
                Message = "Invalid payload",
                Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
            });

        }

        var result = await paymentService.CreatePaymentIntentAsync(dto);
        if (!result.IsSuccess)
        {
            logger.LogWarning("CreatePaymentIntent failed: {Errors}", string.Join(",", result.Errors ?? []));
            return BadRequest(new ApiErrorDto
            {
                Status = 400,
                Message = "Create payment intent failed",
                Errors = result.Errors
            });
        }

        return Ok(result.Value);
    }


    [HttpPost("webhook")]
    public async Task<IActionResult> Webhook()
    {
        var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
        var signatureHeader = Request.Headers["Stripe-Signature"];

        var result = await paymentService.HandleWebhookAsync(json, signatureHeader!);

        if (!result.IsSuccess)
        {
            logger.LogWarning("Webhook failed: {Errors}", string.Join(",", result.Errors ?? []));
            return BadRequest(new ApiErrorDto
            {
                Status = 400,
                Message = "Webhook processing failed",
                Errors = result.Errors
            });
        }

        return Ok();
    }
}
