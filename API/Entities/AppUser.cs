using Microsoft.AspNetCore.Identity;

namespace API.Entities;

public class AppUser : IdentityUser<int>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string SchoolName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; } // date format  YYYY-MM-DD
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Course> CoursesTaught { get; set; } = [];
    public ICollection<Enrollment> Enrollments { get; set; } = [];
    // Link to children if parent
    public ICollection<ParentStudentLink> ChildrenLinks { get; set; } = [];
    // Link to parents if student
    public ICollection<ParentStudentLink> ParentLinks { get; set; } = [];
    public ICollection<AppUserRole> UserRoles { get; set; } = [];
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];

}
