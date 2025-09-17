
namespace API.Entities;

public class Enrollment
{
    public int EnrollmentId { get; set; }
    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;
    public int StudentId { get; set; }
    public AppUser Student { get; set; } = null!;
    public DateTime EnrollmentDate { get; set; }
    public EnrollmentStatus Status { get; set; }

}

public enum EnrollmentStatus
{
    Enrolled = 0,
    Completed = 1,
    Dropped = 2
}
