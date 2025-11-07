
namespace API.DTOs.Carts;

public record PaymentIntentRequestDto
{
    public string CartId { get; set; } = string.Empty;
}
