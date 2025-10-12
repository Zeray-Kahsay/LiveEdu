using API.Data;
using API.Entities;
using API.Entities.Courses;
using API.Interfaces.Enrollments;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.CourseEnrollment;

public class EnrollmentRepository : Repository<Enrollment>, IEnrollmentRepository
{
    public EnrollmentRepository(AppDbContext context) : base(context) { }



    public async Task<IEnumerable<Enrollment>> GetEnrollmentByStudentAsync(int studentId)
    {
        return await _context.Enrollments
            .Include(e => e.Course)
            .ThenInclude(e => e.Teacher)
            .Where(e => e.StudentId == studentId)
            .ToListAsync();
    }

    public async Task<Enrollment?> GetEnrollmentByCourseAndStudentAsync(int courseId, int studentId)
    {
        return await _context.Enrollments
              .Include(e => e.Course)
              .ThenInclude(e => e.Teacher)
              .FirstOrDefaultAsync(e => e.CourseId == courseId && e.StudentId == studentId);
    }

    public async Task AddEnrollmentAsync(Enrollment enrollment)
    {
        await _context.Enrollments.AddAsync(enrollment);
        await _context.SaveChangesAsync();

    }
}
