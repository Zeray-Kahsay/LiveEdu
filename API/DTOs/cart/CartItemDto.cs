namespace API.DTOs.cart;

public record CartItemDto
{
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; } = 1;
}
