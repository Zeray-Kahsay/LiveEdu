using API.Data;
using API.Entities;
using API.Interfaces.CourseEnrollment;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.CourseEnrollment;

public class CourseRepository : Repository<Course>, ICourseRepository
{
    public CourseRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Course>> GetByGradeAndSubjectAsync(GradeLevel gradeLevel, string subject)
    {
        return await _context.Courses
                 .Include(c => c.Teacher)
                 .Include(c => c.Sessions)
                 .Where(c => c.GradeLevel == gradeLevel && c.Subject == subject)
                 .ToListAsync();
    }

    public async Task<IEnumerable<Course>> GetAllWithDetailsAsync()
    {
        return await _context.Courses
            .Include(c => c.Teacher)
            .Include(c => c.Sessions)
            .ToListAsync();
    }


    public async Task<Course?> GetByIdWithDetailsAsync(int id)
    {
        return await _context.Courses
           .Include(c => c.Teacher)
           .Include(c => c.Sessions)
           .FirstOrDefaultAsync(c => c.CourseId == id);

    }
}
