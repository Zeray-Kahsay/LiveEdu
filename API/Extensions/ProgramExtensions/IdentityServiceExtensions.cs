
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Identity;

namespace API.Extensions.ProgramExtensions;

public static class IdentityServiceExtensions
{
    public static IServiceCollection AddIdentityServices(this IServiceCollection services)
    {
        services.AddIdentityCore<AppUser>(opt =>
        {
            opt.Password.RequireNonAlphanumeric = false;
            //opt.Password.RequiredLength = 6;
            opt.Password.RequireDigit = false;
            opt.Password.RequireUppercase = false;
            opt.User.RequireUniqueEmail = true;
            opt.SignIn.RequireConfirmedEmail = false; // Set to true if email confirmation is implemented
        })
        .AddRoles<AppRole>()
        .AddRoleManager<RoleManager<AppRole>>()
        .AddSignInManager<SignInManager<AppUser>>()
        .AddRoleValidator<RoleValidator<AppRole>>()
        .AddEntityFrameworkStores<AppDbContext>();

        return services;
    }

}
