namespace API.DTOs.Account;

public record RegisterDto
{
    public string Username { get; init; } = default!;
    public string FirstName { get; init; } = default!;
    public string LastName { get; init; } = default!;
    public string Email { get; init; } = default!;
    public string Password { get; init; } = default!;
    public string  Role  { get; set; } = default!;
}
