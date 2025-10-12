namespace API.DTOs.Courses;

public class CoursePageDto
{
    public IEnumerable<CourseDto> Courses { get; set; } = [];
    public bool HasNextPage { get; set; }
    public int? LastId { get; set; }
}
