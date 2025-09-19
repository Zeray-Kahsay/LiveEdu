namespace API.DTOs.Account;

public record LoginDto
{
    public string Email { get; init; } = default!;
    public string Password { get; init; } = default!;
}
