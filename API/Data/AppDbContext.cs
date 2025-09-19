
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class AppDbContext : IdentityDbContext
                <AppUser, AppRole, int,
                 IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
                 IdentityRoleClaim<int>, IdentityUserToken<int>>

{
    public AppDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Course> Courses { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<ParentStudentLink> ParentStudentLinks { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Enrollment>()
            .HasKey(e => new { e.StudentId, e.CourseId });

        builder.Entity<Enrollment>()
            .HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId)
            .OnDelete(DeleteBehavior.NoAction); // <- add Restrict to avoid multiple cascade paths

        builder.Entity<Enrollment>()
            .HasOne(e => e.Course)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.Entity<ParentStudentLink>()
            .HasKey(ps => new { ps.ParentId, ps.StudentId });

        builder.Entity<ParentStudentLink>()
            .HasOne(ps => ps.Parent)
            .WithMany(p => p.ChildrenLinks)
            .HasForeignKey(ps => ps.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ParentStudentLink>()
            .HasOne(ps => ps.Student)
            .WithMany(s => s.ParentLinks)
            .HasForeignKey(ps => ps.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<AppUserRole>(userRole =>
        {
            userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

            userRole.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            userRole.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
        });
    }




}
