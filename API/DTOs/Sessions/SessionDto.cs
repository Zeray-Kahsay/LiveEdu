
namespace API.DTOs.Sessions;

public record SessionDto
{
    public int SessionId { get; set; }
    public string Title { get; set; } = default!;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string StreamUrl { get; set; } = default!;
    public bool IsLive { get; set; }
}
