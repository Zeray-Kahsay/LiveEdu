using API.Entities.Courses;

namespace API.Entities.Carts;

public class CartItem
{
    public int Id { get; set; }
    public string CourseTitle { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Quantity { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string GradeLevel { get; set; } = string.Empty;

    // FK and navigation to the course
    public int CourseId { get; set; }
    public Course Course { get; set; } = default!;

    // // Cart reference 
    public int CartId { get; set; }
    public Cart Cart { get; set; } = default!;

}