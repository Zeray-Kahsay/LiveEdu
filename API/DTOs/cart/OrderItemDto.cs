namespace API.DTOs.cart;

public record OrderItemDto
{
    public int CourseId { get; set; }
    public string CourseTitle { get; set; } = default!;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
