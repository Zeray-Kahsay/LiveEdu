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
           .Property(e => e.Status)
           .HasConversion<string>();


        builder.Entity<Course>()
          .HasOne(c => c.Teacher)
          .WithMany(t => t.CoursesTaught)
          .HasForeignKey(c => c.TeacherId)
          .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Session>()
            .HasOne(s => s.Course)
            .WithMany(c => c.Sessions)
            .HasForeignKey(s => s.CourseId);

        builder.Entity<Enrollment>()
            .HasOne(e => e.Student)
            .WithMany(s => s.Enrollments)
            .HasForeignKey(e => e.StudentId);

        builder.Entity<Enrollment>()
            .HasOne(e => e.Course)
            .WithMany(c => c.Enrollments)
            .HasForeignKey(e => e.CourseId);

        builder.Entity<ParentStudentLink>()
            .HasOne(p => p.Parent)
            .WithMany(p => p.ChildrenLinks)
            .HasForeignKey(p => p.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<ParentStudentLink>()
            .HasOne(p => p.Student)
            .WithMany()
            .HasForeignKey(p => p.StudentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Attendance>()
            .HasOne(a => a.Student)
            .WithMany()
            .HasForeignKey(a => a.StudentId);

        builder.Entity<Attendance>()
            .HasOne(a => a.Session)
            .WithMany()
            .HasForeignKey(a => a.SessionId);

        builder.Entity<Submission>()
            .HasOne(s => s.Student)
            .WithMany()
            .HasForeignKey(s => s.StudentId);

        builder.Entity<Submission>()
            .HasOne(s => s.Assignment)
            .WithMany()
            .HasForeignKey(s => s.AssignmentId);

        // M-M
        builder.Entity<AppUser>()
                     .HasMany(au => au.UserRoles)
                     .WithOne(ur => ur.User)
                     .HasForeignKey(ur => ur.UserId)
                     .IsRequired();

        builder.Entity<AppRole>()
               .HasMany(ap => ap.UserRoles)
               .WithOne(ur => ur.Role)
               .HasForeignKey(ar => ar.RoleId)
               .IsRequired();


    }




}
