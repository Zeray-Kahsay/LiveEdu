
using API.Entities.Courses;

namespace API.Entities.Users;

public class Attendance
{
    public int AttendanceId { get; set; }

    public int SessionId { get; set; }
    public Session Session { get; set; } = null!;

    public int StudentId { get; set; }
    public AppUser Student { get; set; } = null!;

    public DateTime JoinedAt { get; set; }
    public DateTime LeftAt { get; set; }

}
