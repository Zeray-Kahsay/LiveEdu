using System;

namespace API.DTOs.Account.User;

public record UserDto
{
    public int Id { get; set; }
    //public string? Username { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? SchoolName { get; set; }
    public string? Token { get; set; }
    public string? RefreshToken { get; set; }
}
