namespace API.DTOs.Accounts.User;

public record ChangePasswordDto
{
    public string Email { get; set; } = default!;
    public string CurrentPassword { get; init; } = default!;
    public string NewPassword { get; init; } = default!;
}
