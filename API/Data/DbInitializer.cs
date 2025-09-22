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

        // Seed sample users from JSON as students
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

        // Assign teacher (use the seeded "teacher@gmail.com")
        var teacher = await userManager.FindByEmailAsync("teacher@gmail.com");
        if (teacher == null) return;

        var courses = new List<Course>
      {
        new Course
       {
        Title = "Intro to Math",
        Subject = "Math",
        GradeLevel = GradeLevel.Grade1,
        Description = "Fun with numbers!",
        TeacherId = teacher.Id,
        Sessions = new List<Session>
        {
            new Session
            {
                Title = "Counting Basics",
                StartTime = DateTime.UtcNow.AddDays(1).AddHours(9),
                EndTime = DateTime.UtcNow.AddDays(1).AddHours(10),
                StreamUrl = "https://streamingplatform.com/math/counting",
                IsLive = false
            },
            new Session
            {
                Title = "Addition Made Easy",
                StartTime = DateTime.UtcNow.AddDays(3).AddHours(9),
                EndTime = DateTime.UtcNow.AddDays(3).AddHours(10),
                StreamUrl = "https://streamingplatform.com/math/addition",
                IsLive = false
            }
        }
       },
        new Course
       {
        Title = "Advanced Algebra",
        Subject = "Math",
        GradeLevel = GradeLevel.Grade8,
        Description = "Equations and more",
        TeacherId = teacher.Id,
        Sessions = new List<Session>
        {
            new Session
            {
                Title = "Linear Equations",
                StartTime = DateTime.UtcNow.AddDays(2).AddHours(11),
                EndTime = DateTime.UtcNow.AddDays(2).AddHours(12),
                StreamUrl = "https://streamingplatform.com/algebra/linear",
                IsLive = false
            },
            new Session
            {
                Title = "Quadratic Equations",
                StartTime = DateTime.UtcNow.AddDays(4).AddHours(11),
                EndTime = DateTime.UtcNow.AddDays(4).AddHours(12),
                StreamUrl = "https://streamingplatform.com/algebra/quadratic",
                IsLive = false
            }
        }
       },
       new Course
       {
        Title = "Biology Basics",
        Subject = "Science",
        GradeLevel = GradeLevel.Grade6,
        Description = "Introduction to life science",
        TeacherId = teacher.Id,
        Sessions = new List<Session>
        {
            new Session
            {
                Title = "Introduction to Cells",
                StartTime = DateTime.UtcNow.AddDays(1).AddHours(14),
                EndTime = DateTime.UtcNow.AddDays(1).AddHours(16),
                StreamUrl = "https://streamingplatform.com/biology/cells",
                IsLive = false
            },
            new Session
            {
                Title = "The Human Body",
                StartTime = DateTime.UtcNow.AddDays(3).AddHours(14),
                EndTime = DateTime.UtcNow.AddDays(3).AddHours(16),
                StreamUrl = "https://streamingplatform.com/biology/body",
                IsLive = false
            }
        }
       },
      new Course
      {
        Title = "World History",
        Subject = "History",
        GradeLevel = GradeLevel.Grade10,
        Description = "Explore past civilizations",
        TeacherId = teacher.Id,
        Sessions = new List<Session>
        {
            new Session
            {
                Title = "Ancient Egypt",
                StartTime = DateTime.UtcNow.AddDays(2).AddHours(10),
                EndTime = DateTime.UtcNow.AddDays(2).AddHours(11),
                StreamUrl = "https://streamingplatform.com/history/egypt",
                IsLive = false
            },
            new Session
            {
                Title = "The Roman Empire",
                StartTime = DateTime.UtcNow.AddDays(4).AddHours(10),
                EndTime = DateTime.UtcNow.AddDays(4).AddHours(11),
                StreamUrl = "https://streamingplatform.com/history/rome",
                IsLive = false
            }
        }
    }
};



        context.Courses.AddRange(courses);
        await context.SaveChangesAsync();

        // Enroll a few students in the first course
        var students = await userManager.Users
            .Where(u => u.Email != null && u.Email != "teacher@gmail.com" && u.Email != "admin@gmail.com")
            .OrderBy(u => u.Id)
            .Take(5)
            .ToListAsync();

        foreach (var student in students)
        {
            if (await userManager.IsInRoleAsync(student, "Student"))
            {
                context.Enrollments.Add(new Enrollment
                {
                    CourseId = courses.First().CourseId,
                    StudentId = student.Id,
                    Status = EnrollmentStatus.Enrolled
                });
            }
        }

        await context.SaveChangesAsync();
    }
}
