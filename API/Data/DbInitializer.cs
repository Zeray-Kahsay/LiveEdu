
using API.Entities;
using Microsoft.AspNetCore.Identity;

namespace API.Data;

public static class DbInitializer
{
    public static async Task Initialize
    (
        AppDbContext context,
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager
        )
    {
        // Ensure database is created
        context.Database.EnsureCreated();

        // Check if roles exist, if not create them
        if (!roleManager.Roles.Any())
        {
            var roles = new List<AppRole>
            {
                new() { Name = "Admin" },
                new() { Name = "Teacher" },
                new () { Name = "Student" },
                new () { Name = "Parent" }
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }
        }

        // Check if users exist, if not create default users
        if (!userManager.Users.Any())
        {
            var adminUser = new AppUser
            {
                UserName = "admin",
                Email = "admin@gmail.com",
            };
            await userManager.CreateAsync(adminUser, "Admin123!");
            await userManager.AddToRoleAsync(adminUser, "Admin");

            var teacherUser = new AppUser
            {
                UserName = "teacher",
                Email = "teacher1@gmail.com",
            };
            await userManager.CreateAsync(teacherUser, "Teacher123!");
            await userManager.AddToRoleAsync(teacherUser, "Teacher");
            var studentUser = new AppUser
            {
                UserName = "student",
                Email = " student1@gmail.com",
            };
            await userManager.CreateAsync(studentUser, "Student123!");
            await userManager.AddToRoleAsync(studentUser, "Student");
            var parentUser = new AppUser
            {
                UserName = "parent",
                Email = "parent1@gmail.com",
            };
            await userManager.CreateAsync(parentUser, "Parent123!");
            await userManager.AddToRoleAsync(parentUser, "Parent");
        }



        await context.SaveChangesAsync();
    }
}
