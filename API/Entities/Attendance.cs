
namespace API.Entities;

public class Attendance
{
    public int AttendanceId { get; set; }
    public int SessionId { get; set; }
    public int StudentId { get; set; }
    public DateTime JoinTime { get; set; }
    public DateTime LeaveTime { get; set; }

}
