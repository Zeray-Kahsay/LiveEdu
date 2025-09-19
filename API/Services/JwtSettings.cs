namespace API.Services;

public class JwtSettings
{
    public string TokenKey { get; set; } = default!;
    public string Issuer { get; set; } = default!;
    public string Audience { get; set; } = default!;
    public int ExpiryInMinutes { get; set; } = default!;
}
