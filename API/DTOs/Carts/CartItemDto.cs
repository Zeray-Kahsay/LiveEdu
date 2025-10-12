namespace API.DTOs.Carts;

public record CartItemDto
{
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string? Subject { get; set; }
    public string? GradeLevel { get; set; }
    public string? TeacherName { get; set; }
    public string? Description { get; set; }
}
