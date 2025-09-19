using Microsoft.AspNetCore.Identity;

namespace API.Entities;

public class AppUser : IdentityUser<int>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public DateTime CreatedAt { get; set; }
    public ICollection<Course> CoursesTaught { get; set; } = [];
    public ICollection<Enrollment> Enrollments { get; set; } = [];
    // Link to children if parent
    public ICollection<ParentStudentLink> ChildrenLinks { get; set; } = [];
    // Link to parents if student
    public ICollection<ParentStudentLink> ParentLinks { get; set; } = [];
    public ICollection<AppUserRole> UserRoles { get; set; } = [];

}
