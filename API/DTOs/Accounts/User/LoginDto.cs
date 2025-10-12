namespace API.DTOs.Accounts.User;

public record LoginDto
{
    public string Email { get; init; } = default!;
    public string Password { get; init; } = default!;
    public string DeviceId { get; set; } = default!;
}
