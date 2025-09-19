namespace API.Services;

public interface ITokenService
{
    Task<string> CreateToken(AppUser user);
}

public class TokenService
(
    SymmetricSecurityKey key,
    UserManager<AppUser> userManager,
    IOptions<JwtSettings> settings) : ITokenService
{
    public async Task<string> CreateToken(AppUser user)
    {
        var claims = new List<Claim>
        {
          new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
          new(ClaimTypes.NameIdentifier, user.Id.ToString()),
          new(JwtRegisteredClaimNames.UniqueName, user.UserName ?? string.Empty),
          new(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
          new("firstName", user.FirstName ?? string.Empty),
          new("lastName", user.LastName ?? string.Empty)

        };

        var roles = await userManager.GetRolesAsync(user);
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(settings.Value.ExpiryInMinutes),
            SigningCredentials = creds,
            Issuer = settings.Value.Issuer,
            Audience = settings.Value.Audience
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
