namespace API.DTOs.Accounts.User;

public record ResetPasswordDto
{
    public string Email { get; init; } = default!;
    public string Token { get; init; } = default!;
    public string NewPassword { get; init; } = default!;
}
