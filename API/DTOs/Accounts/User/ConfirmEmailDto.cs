namespace API.DTOs.Accounts.User;

public record ConfirmEmailDto
{
    public string Email { get; init; } = default!;
    public string Token { get; init; } = default!;
}
