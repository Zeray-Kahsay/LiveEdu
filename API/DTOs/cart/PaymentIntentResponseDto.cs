namespace API.DTOs.cart;

public record PaymentIntentResponseDto
{
    public string ClientSecret { get; set; } = default!;
    public string PaymentIntentId { get; set; } = default!;
    public string PublishableKey { get; set; } = default!;
}
