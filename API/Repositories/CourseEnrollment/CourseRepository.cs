using API.Data;
using API.DTOs.Course;
using API.Entities;
using API.Helpers;
using API.Interfaces.CourseEnrollment;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.CourseEnrollment;

public class CourseRepository : Repository<Course>, ICourseRepository
{
    public CourseRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Course>> GetByGradeAndSubjectAsync(CourseFilterDto filter)
    {
        var query = _context.Courses
                 .Include(c => c.Teacher)
                 .Include(c => c.Sessions)
                 .Where(c => c.GradeLevel == filter.GradeLevel && c.Subject == filter.Subject)
                 .AsQueryable();

        if (filter.GradeLevel.HasValue)
        {
            query = query.Where(c => c.GradeLevel == filter.GradeLevel);
        }

        if (!string.IsNullOrEmpty(filter.Subject))
        {
            query = query.Where(c => c.Subject == filter.Subject);
        }

        return await query.ToListAsync();
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

    public async Task<List<Course>> GetCoursesAsync(CourseParams courseParams)
    {
        var query = _context.Courses
            .Include(c => c.Teacher)
            .Include(c => c.Sessions)
            .AsQueryable();

        // Searching
        if (!string.IsNullOrEmpty(courseParams.SearchTerm))
        {
            //var term = courseParams.SearchTerm.ToLower();
            query = query.Where(c =>
               c.Title.Contains(courseParams.SearchTerm, StringComparison.CurrentCultureIgnoreCase) ||
               c.Subject.Contains(courseParams.SearchTerm, StringComparison.CurrentCultureIgnoreCase));
        }

        // Filter by grade
        if (courseParams.GradeLevel.HasValue)
        {
            query = query.Where(c => c.GradeLevel == courseParams.GradeLevel.Value);
        }

        // Filtering by subject
        if (!string.IsNullOrEmpty(courseParams.Subject))
        {
            query = query.Where(c => c.Subject == courseParams.Subject);
        }

        // cursor condition
        if (courseParams.LastId.HasValue)
        {
            query = query.Where(c => c.CourseId > courseParams.LastId.Value);
        }

        // Fetch +1 for 'has next'
        var items = await query
                 .OrderBy(c => c.CourseId)
                 .Take(courseParams.PageSize + 1)
                 .ToListAsync();

        return items;
    }
}
