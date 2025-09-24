using API.Data;
using API.Entities;
using API.Interfaces.CourseEnrollment;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.CourseEnrollment;

public class EnrollmentRepository : Repository<Enrollment>, IEnrollmentRepository
{
    public EnrollmentRepository(AppDbContext context) : base(context) { }



    public async Task<IEnumerable<Enrollment>> GetEnrollmentByStudentAsync(int studentId)
    {
        return await _context.Enrollments
            .Include(e => e.Course)
            .Where(e => e.StudentId == studentId)
            .ToListAsync();
    }

    public async Task<Enrollment?> GetEnrollmentByCourseAndStudentAsync(int courseId, int studentId)
    {
        return await _context.Enrollments
              .FirstOrDefaultAsync(e => e.CourseId == courseId && e.StudentId == studentId);
    }

}
