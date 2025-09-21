
using System.Text.Json;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public static class DbInitializer
{
    public static async Task Initialize
    (
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager
        )
    {

        if (await userManager.Users.AnyAsync()) return;

        var userData = await File.ReadAllTextAsync("Data/UserData.json");

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

        if (users is null) return;

        var roles = new List<AppRole>
       {
        new(){ Name = "Admin"},
        new(){ Name = "Teacher"},
        new(){ Name = "Student"},
        new(){ Name = "Parent"},
       };

        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }

        foreach (var user in users)
        {
            user.UserName = user.UserName!.ToLower();
            user.SecurityStamp = new Guid().ToString();
            await userManager.CreateAsync(user, "Pa$$w0rd");
            await userManager.AddToRoleAsync(user, "Student");
        }

        var admin = new AppUser
        {
            FirstName = "Adminstrator",
            LastName = "Adminstrator",
            UserName = "admin@gmail.com",
            Email = "admin@gmail.com",
            CreatedAt = DateTime.UtcNow,
            SecurityStamp = new Guid().ToString()
        };
        await userManager.CreateAsync(admin, "Pa$$w0rd");
        await userManager.AddToRolesAsync(admin, ["Teacher", "Student", "Admin", "Parent"]);

        var teacher = new AppUser
        {
            FirstName = "Teacher",
            LastName = "Teacher",
            UserName = "teacher@gmail.com",
            Email = "teacher@gmail.com",
            CreatedAt = DateTime.UtcNow,
            SecurityStamp = new Guid().ToString()
        };
        await userManager.CreateAsync(teacher, "Pa$$w0rd");
        await userManager.AddToRolesAsync(teacher, ["Teacher"]);


    }
}
