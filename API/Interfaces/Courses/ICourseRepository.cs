using API.DTOs.Courses;
using API.Entities;
using API.Entities.Courses;
using API.Helpers;

namespace API.Interfaces.Courses;

public interface ICourseRepository : IRepository<Course>
{
    Task<IEnumerable<Course>> GetByGradeAndSubjectAsync(CourseFilterDto filter);
    Task<List<Course>> GetCoursesAsync(CourseParams courseParams);
    Task<IEnumerable<Course>> GetAllWithDetailsAsync();
    Task<Course?> GetByIdWithDetailsAsync(int id);

    // For teacher 
    Task AddCourseAsync(Course course);
    Task<IEnumerable<Course>> GetCoursesByTeacherAsync(int teacherId);
    Task<bool> SaveAllAsync();
}
