namespace API.DTOs.Orders;

public record OrderDto
{
    public int OrderId { get; set; }
    public int? UserId { get; set; }
    public decimal Total { get; set; }
    public DateTime CreatedAt { get; set; }
    public bool IsPaid { get; set; }
    public List<OrderItemDto> Items { get; set; } = [];
}

