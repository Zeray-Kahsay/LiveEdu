using API.Entities;

namespace API.DTOs.Course;

public record CourseFilterDto
{
    public GradeLevel? GradeLevel { get; set; }
    public string? Subject { get; set; }
}
