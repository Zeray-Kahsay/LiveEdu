using Microsoft.AspNetCore.Identity;

namespace API.Entities.Users;

public class AppRole : IdentityRole<int>
{
    public ICollection<AppUserRole> UserRoles { get; set; } = [];
}
