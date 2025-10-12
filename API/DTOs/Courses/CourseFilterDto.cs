using API.Entities;
using API.Entities.Courses;

namespace API.DTOs.Courses;

public record CourseFilterDto
{
    public GradeLevel? GradeLevel { get; set; }
    public string? Subject { get; set; }
}
