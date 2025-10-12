
namespace API.Entities.Courses;

public class Assignment
{
    public int AssignmentId { get; set; }
    public int SessionId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }

}
