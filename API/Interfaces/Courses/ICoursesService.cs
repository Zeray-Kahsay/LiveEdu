using API.DTOs.Courses;
using API.Entities.Courses;
using API.Helpers;

namespace API.Interfaces.Courses;

public interface ICoursesService
{
    Task<Result<CoursePageDto>> GetCoursesAsync(CourseParams courseParams);
    Task<Result<CourseDto>> GetCourseByIdAsync(int id);
    Task<Result<IEnumerable<CourseDto>>> GetByGradeAndSubjectAsync(CourseFilterDto filter);
    // For teacher
    Task<Result<Course>> AddCourseAsync(CourseCreateDto createCourseDto, int teacherId);
}
