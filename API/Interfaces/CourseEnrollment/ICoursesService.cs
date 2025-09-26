using API.DTOs.Course;
using API.Entities;
using API.Helpers;

namespace API.Interfaces.CourseEnrollment;

public interface ICoursesService
{
    Task<Result<IEnumerable<CourseDto>>> GetCoursesAsync();
    Task<Result<CourseDto>> GetCourseByIdAsync(int id);
    Task<Result<IEnumerable<CourseDto>>> GetByGradeAndSubjectAsync(CourseFilterDto filter);
}
