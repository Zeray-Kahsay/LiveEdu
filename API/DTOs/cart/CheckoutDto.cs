using API.DTOs.Course;

namespace API.DTOs.cart;

public class CheckoutDto
{
    public int OrderId { get; set; }
    public List<CourseDto> Courses { get; set; } = [];
    public bool IsPaid { get; set; }
}
