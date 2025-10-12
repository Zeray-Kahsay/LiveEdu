using API.DTOs.Sessions;

namespace API.DTOs.Enrollments;

public record EnrollmentDto
{
    public int EnrollmentId { get; set; }
    public int CourseId { get; set; }
    public string Description { get; set; } = string.Empty;
    public string CourseTitle { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string GradeLevel { get; set; } = string.Empty;
    public string TeacherName { get; set; } = string.Empty;
    public DateTime EnrolledAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<SessionDto> Sessions { get; set; } = [];
}
