using API.DTOs.Carts;

namespace API.DTOs.Payments;

public record CreatePaymentDto
{
    public List<CartItemDto> Items { get; set; } = [];
    public string Currency { get; set; } = "usd";
    public int? UserId { get; set; }
}
