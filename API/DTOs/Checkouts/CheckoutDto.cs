using API.DTOs.Courses;

namespace API.DTOs.Checkouts;

public class CheckoutDto
{
    public int OrderId { get; set; }
    public List<CourseDto> Courses { get; set; } = [];
    public bool IsPaid { get; set; }
}
