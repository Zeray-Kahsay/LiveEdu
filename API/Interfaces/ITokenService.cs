
using API.DTOs.Account.User;
using API.Entities;

namespace API.Interfaces;

public interface ITokenService
{
    Task<string> CreateToken(AppUser user, string deviceId);
    Task<RefreshToken> CreateRefreshToken(AppUser appUser, string deviceId);
    Task<AuthResponseDto> GenerateAuthResponse(AppUser user, string deviceId);

}
