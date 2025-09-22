
using API.Entities;

namespace API.Interfaces;

public interface ITokenService
{
    Task<string> CreateToken(AppUser user, string deviceId);
    Task<string> CreateRefreshToken(AppUser appUser, string deviceId);

}
