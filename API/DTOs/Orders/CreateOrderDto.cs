using API.DTOs.Carts;

namespace API.DTOs.Orders;

public class CreateOrderDto
{
    public int? UserId { get; set; }
    public List<CartItemDto> Items { get; set; } = [];
    public string Currency { get; set; } = "usd";
}
