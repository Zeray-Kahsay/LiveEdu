using System.ComponentModel.DataAnnotations;

namespace API.DTOs.Accounts.User;

public record RegisterDto
{
    [Required]
    public string FirstName { get; init; } = default!;
    [Required]
    public string LastName { get; init; } = default!;
    [Required]
    public string Email { get; init; } = default!;
    [Required]
    public string Password { get; init; } = default!;
    [Required]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    public string ConfirmPassword { get; set; } = default!;
    [Required]
    public string SchoolName { get; set; } = default!;
    [Required]
    public string Role { get; set; } = default!;
}
