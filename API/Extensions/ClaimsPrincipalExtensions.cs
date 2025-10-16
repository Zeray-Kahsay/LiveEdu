
using System.Security.Claims;

namespace API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static string GetUserName(this ClaimsPrincipal user)
    {
        var username = user.FindFirstValue(ClaimTypes.Name) ??
            throw new Exception("User has no username claim");
        return username;
    }

    public static int GetUserId(this ClaimsPrincipal user)
    {
        var id = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier) ??
            throw new Exception("User has no name identifier claim"));
        return id;
    }
}
