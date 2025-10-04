using API.Entities;

namespace API.DTOs.Course;

public class CourseDto
{
    public int Id { get; set; }
    public string Title { get; set; } = default!;
    public string Description { get; set; } = default!;
    public double Price { get; set; }
    public string Subject { get; set; } = default!;
    public string GradeLevel { get; set; } = default!;
    public string TeacherName { get; set; } = default!;
    public List<SessionDto> Sessions { get; set; } = [];
    //public DateTime StartDate { get; set; }
    //public DateTime EndDate { get; set; }
}
