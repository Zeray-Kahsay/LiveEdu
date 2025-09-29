using API.DTOs.Course;
using API.Entities;
using API.Helpers;

namespace API.Interfaces.CourseEnrollment;

public interface ICourseRepository : IRepository<Course>
{
    Task<IEnumerable<Course>> GetByGradeAndSubjectAsync(CourseFilterDto filter);
    Task<List<Course>> GetCoursesAsync(CourseParams courseParams);
    Task<IEnumerable<Course>> GetAllWithDetailsAsync();
    Task<Course?> GetByIdWithDetailsAsync(int id);
}
