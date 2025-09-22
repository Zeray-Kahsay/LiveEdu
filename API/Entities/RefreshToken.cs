namespace API.Entities;

public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime Expires { get; set; }
    public bool IsRevoked { get; set; } = false;
    public string DeviceId { get; set; } = string.Empty; // identifies device/browser
    public int UserId { get; set; }
    public AppUser User { get; set; } = null!;
}

