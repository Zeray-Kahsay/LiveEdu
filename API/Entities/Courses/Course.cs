
using API.Entities.Users;

namespace API.Entities.Courses;

public class Course
{
    public int CourseId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int MaxStudents { get; set; }
    public GradeLevel GradeLevel { get; set; }
    public string Subject { get; set; } = string.Empty;

    public int TeacherId { get; set; }
    public AppUser Teacher { get; set; } = null!;

    public ICollection<Session> Sessions { get; set; } = [];
    public ICollection<Enrollment> Enrollments { get; set; } = [];





}
