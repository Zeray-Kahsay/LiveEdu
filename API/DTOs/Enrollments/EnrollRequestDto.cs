namespace API.DTOs.Enrollments;

public record EnrollRequestDto
{
    public int CourseId { get; set; }
    public int StudentId { get; set; }
}
