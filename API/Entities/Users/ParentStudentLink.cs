
namespace API.Entities.Users;

public class ParentStudentLink
{
    public int Id { get; set; }

    public int ParentId { get; set; }
    public AppUser Parent { get; set; } = null!;

    public int StudentId { get; set; }
    public AppUser Student { get; set; } = null!;

    public DateTime LinkedAt { get; set; } = DateTime.UtcNow;

}
