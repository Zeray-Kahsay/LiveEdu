using API.Helpers;
using API.Interfaces.CourseCart;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class PaymentController(IPaymentService paymentService, ILogger<PaymentController> logger) : ControllerBase
{
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

        return Ok(new { status = "success" });
    }
}
