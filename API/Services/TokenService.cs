using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using API.Data;
using API.DTOs.Accounts.User;
using API.Entities.Users;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace API.Services;



public class TokenService : ITokenService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SymmetricSecurityKey _key;
    private readonly JwtSettings _settings;
    private readonly AppDbContext _context;

    public TokenService(UserManager<AppUser> userManager, IOptions<JwtSettings> settings, AppDbContext context)
    {
        _userManager = userManager;
        _settings = settings.Value;
        var tokenKey = _settings.TokenKey;
        if (string.IsNullOrEmpty(tokenKey))
            throw new ArgumentNullException(nameof(tokenKey), "Token key is missing in JWT settings");
        _key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(tokenKey));
        _context = context;

    }


    public async Task<string> CreateToken(AppUser user, string deviceId)
    {
        if (user.Email is null) return $"No user with {user.Email} found";


        var claims = new List<Claim>
        {
          new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
          new(ClaimTypes.NameIdentifier, user.Id.ToString()),
          new(JwtRegisteredClaimNames.UniqueName, user.UserName ?? string.Empty),
          new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
          new("firstName", user.FirstName ?? string.Empty),
          new("lastName", user.LastName ?? string.Empty),
          new("deviceId", deviceId)

        };

        var roles = await _userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_settings.ExpiryInMinutes),
            SigningCredentials = creds,
            Issuer = _settings.Issuer,
            Audience = _settings.Audience
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }


    public async Task<RefreshToken> CreateRefreshToken(AppUser appUser, string deviceId)
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.UtcNow.AddDays(7),
            UserId = appUser.Id,
            DeviceId = deviceId
        };

        _context.RefreshTokens.Add(refreshToken);
        await _context.SaveChangesAsync();

        return refreshToken;
    }

    public async Task<AuthResponseDto> GenerateAuthResponse(AppUser user, string deviceId)
    {
        var accessToken = await CreateToken(user, deviceId);
        var refreshToken = await CreateRefreshToken(user, deviceId);

        var userDto = new UserDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
            SchoolName = user.SchoolName,
            Roles = (await _userManager.GetRolesAsync(user)).ToList(),
            Token = accessToken,
            RefreshToken = refreshToken.Token
        };

        return new AuthResponseDto
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken.Token,
            User = userDto
        };

    }
}
