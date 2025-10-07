namespace API.DTOs.cart;

public record OrderDto
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public string PaymentIntentId { get; set; } = default!;
    public decimal Total { get; set; }
    public bool IsPaid { get; set; }
    public List<OrderItemDto> Items { get; set; } = [];
}

