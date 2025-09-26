using API.DTOs.Course;
using API.Entities;

namespace API.Interfaces.CourseEnrollment;

public interface ICourseRepository : IRepository<Course>
{
    Task<IEnumerable<Course>> GetByGradeAndSubjectAsync(CourseFilterDto filter);
    Task<IEnumerable<Course>> GetAllWithDetailsAsync();
    Task<Course?> GetByIdWithDetailsAsync(int id);
}
