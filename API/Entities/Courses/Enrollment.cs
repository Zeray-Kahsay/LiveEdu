
using API.Entities.Users;

namespace API.Entities.Courses;

public class Enrollment
{
    public int EnrollmentId { get; set; }

    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;

    public int StudentId { get; set; }
    public AppUser Student { get; set; } = null!;

    public DateTime EnrolledAt { get; set; }
    public EnrollmentStatus Status { get; set; }
    public bool IsActive => Status == EnrollmentStatus.Enrolled;

}

public enum EnrollmentStatus
{
    Enrolled = 0,
    Completed = 1,
    Dropped = 2
}
