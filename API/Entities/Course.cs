
namespace API.Entities;

public class Course
{
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public ICollection<Enrollment> Enrollments { get; set; } = [];
    public int MaxStudents { get; set; }
    public int GradeLevel { get; set; }
    public int TeacherId { get; set; }
    public AppUser Teacher { get; set; } = null!;
    public ICollection<Session> Sessions { get; set; } = [];





}
