namespace API.DTOs.Payments;

public record CreatePaymentIntentDto
{
    public int OrderId { get; set; }
    public string Currency { get; set; } = "usd";
}
