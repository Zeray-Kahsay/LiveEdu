
namespace API.Entities.Courses;

public class Session
{
    public int SessionId { get; set; }
    public int CourseId { get; set; }
    public Course Course { get; set; } = null!;
    public string Title { get; set; } = default!;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string StreamUrl { get; set; } = string.Empty;
    public bool IsLive { get; set; }
}
