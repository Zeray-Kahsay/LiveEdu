namespace API.DTOs.Account;

public record ConfirmEmailDto
{
    public string Email { get; init; } = default!;
    public string Token { get; init; } = default!;
}
