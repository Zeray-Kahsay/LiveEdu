namespace API.DTOs.cart;

public record CreatePaymentDto
{
    public List<CartItemDto> Items { get; set; } = [];
    public string Currency { get; set; } = "NOK";
    public int? UserId { get; set; } // ???
}
