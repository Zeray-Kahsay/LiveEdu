namespace API.DTOs.Account.User;

public class RefreshTokenRequestDto
{
    public string RefreshToken { get; set; } = default!;
    public string DeviceId { get; set; } = default!;

}
