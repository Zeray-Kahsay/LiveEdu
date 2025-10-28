namespace API.DTOs.Carts;

public class PaymentIntentResponseDto
{
    public required string ClientSecret { get; set; }
    public required string PublishableKey { get; set; }
}
