using API.DTOs.cart;
using API.Helpers;

namespace API.Interfaces.CourseCart;

public interface IPaymentService
{
    Task<Result<PaymentIntentResponseDto>> CreatePaymentIntentAsync(CreatePaymentDto dto);
    Task<Result> HandleWebhookAsync(string json, string signHeader);

}
