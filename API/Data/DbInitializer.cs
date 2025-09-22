using System.Text.Json;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


public static class DbInitializer
{
    public static async Task Initialize(
        UserManager<AppUser> userManager,
        RoleManager<AppRole> roleManager)
    {
        if (await userManager.Users.AnyAsync()) return;

        var userData = await File.ReadAllTextAsync("Data/UserData.json");
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

        if (users is null) return;

        var roles = new List<AppRole>
        {
            new() { Name = "Admin" },
            new() { Name = "Teacher" },
            new() { Name = "Student" },
            new() { Name = "Parent" }
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role.Name!))
            {
                await roleManager.CreateAsync(role);
            }
        }

        // Seed sample users from JSON
        foreach (var user in users)
        {
            user.Email = user.Email!.ToLower();
            user.UserName = user.UserName!.ToLower();
            user.CreatedAt = DateTime.UtcNow;
            user.SecurityStamp = Guid.NewGuid().ToString();

            await userManager.CreateAsync(user, "Pa$$w0rd");
            await userManager.AddToRoleAsync(user, "Student");
        }

        // Seed admin
        var admin = new AppUser
        {
            FirstName = "Administrator",
            LastName = "Administrator",
            UserName = "admin@gmail.com",
            Email = "admin@gmail.com",
            CreatedAt = DateTime.UtcNow,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        if (await userManager.FindByEmailAsync(admin.Email) == null)
        {
            await userManager.CreateAsync(admin, "Pa$$w0rd");
            await userManager.AddToRolesAsync(admin, new[] { "Teacher", "Student", "Admin", "Parent" });
        }

        // Seed teacher
        var teacher = new AppUser
        {
            FirstName = "Teacher",
            LastName = "Teacher",
            UserName = "teacher@gmail.com",
            Email = "teacher@gmail.com",
            CreatedAt = DateTime.UtcNow,
            SecurityStamp = Guid.NewGuid().ToString()
        };

        if (await userManager.FindByEmailAsync(teacher.Email) == null)
        {
            await userManager.CreateAsync(teacher, "Pa$$w0rd");
            await userManager.AddToRoleAsync(teacher, "Teacher");
        }
    }

    public static async Task SeedCoursesAsync(
    AppDbContext context,
    UserManager<AppUser> userManager)
    {
        if (await context.Courses.AnyAsync()) return;

        var coursesData = await File.ReadAllTextAsync("Data/CourseData.json");
        var sessionsData = await File.ReadAllTextAsync("Data/SessionData.json");

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var courses = JsonSerializer.Deserialize<List<Course>>(coursesData, options);
        var sessions = JsonSerializer.Deserialize<List<Session>>(sessionsData, options);

        if (courses == null || sessions == null) return;

        // Assign teacher (use the seeded "teacher@gmail.com")
        var teacher = await userManager.FindByEmailAsync("teacher@gmail.com");
        if (teacher == null) return;

        for (int i = 0; i < courses.Count; i++)
        {
            var course = courses[i];
            course.TeacherId = teacher.Id;
            course.Sessions = new List<Session> { sessions[i] };
            context.Courses.Add(course);
        }

        await context.SaveChangesAsync();

        // Get candidate users from DB
        var allUsers = await userManager.Users
                   .OrderBy(u => u.Id)
                   .Take(20).ToListAsync();

        var students = new List<AppUser>();
        foreach (var user in allUsers)
        {
            if (await userManager.IsInRoleAsync(user, "Student"))
                students.Add(user);

            if (students.Count == 5) break;
        }



        foreach (var student in students)
        {
            context.Enrollments.Add(new Enrollment
            {
                CourseId = 1, // enroll into "Mathematics Basics"
                StudentId = student.Id,
                Status = EnrollmentStatus.Enrolled
            });
        }

        await context.SaveChangesAsync();
    }

}


// namespace API.Data;

// public static class DbInitializer
// {
//     public static async Task Initialize
//     (
//         UserManager<AppUser> userManager,
//         RoleManager<AppRole> roleManager
//         )
//     {

//         if (await userManager.Users.AnyAsync()) return;

//         var userData = await File.ReadAllTextAsync("Data/UserData.json");

//         var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
//         var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

//         if (users is null) return;

//         var roles = new List<AppRole>
//        {
//         new(){ Name = "Admin"},
//         new(){ Name = "Teacher"},
//         new(){ Name = "Student"},
//         new(){ Name = "Parent"},
//        };

//         foreach (var role in roles)
//         {
//             await roleManager.CreateAsync(role);
//         }

//         foreach (var user in users)
//         {
//             user.UserName = user.UserName!.ToLower();
//             user.SecurityStamp = new Guid().ToString();
//             await userManager.CreateAsync(user, "Pa$$w0rd");
//             await userManager.AddToRoleAsync(user, "Student");
//         }

//         var admin = new AppUser
//         {
//             FirstName = "Adminstrator",
//             LastName = "Adminstrator",
//             UserName = "admin@gmail.com",
//             Email = "admin@gmail.com",
//             CreatedAt = DateTime.UtcNow,
//             SecurityStamp = new Guid().ToString()
//         };
//         await userManager.CreateAsync(admin, "Pa$$w0rd");
//         await userManager.AddToRolesAsync(admin, ["Teacher", "Student", "Admin", "Parent"]);

//         var teacher = new AppUser
//         {
//             FirstName = "Teacher",
//             LastName = "Teacher",
//             UserName = "teacher@gmail.com",
//             Email = "teacher@gmail.com",
//             CreatedAt = DateTime.UtcNow,
//             SecurityStamp = new Guid().ToString()
//         };
//         await userManager.CreateAsync(teacher, "Pa$$w0rd");
//         await userManager.AddToRolesAsync(teacher, ["Teacher"]);


//     }
// }
