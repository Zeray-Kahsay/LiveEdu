using API.Data;
using API.Entities;
using API.Repositories;
using Microsoft.EntityFrameworkCore;

namespace API.Interfaces;

public class CourseRepository : Repository<Course>, ICourseRepository
{
    public CourseRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Course>> GetByGradeAndSubjectAsync(GradeLevel gradeLevel, string subject)
    {
        return await _context.Courses
                 .Where(c => c.GradeLevel == gradeLevel && c.Subject == subject)
                 .ToListAsync();
    }
}
