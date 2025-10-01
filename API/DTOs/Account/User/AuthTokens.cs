namespace API.DTOs.Account.User;

public record AuthTokens
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}
