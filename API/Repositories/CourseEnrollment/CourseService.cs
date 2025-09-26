using API.DTOs.Course;
using API.Entities;
using API.Helpers;
using API.Interfaces.CourseEnrollment;

namespace API.Repositories.CourseEnrollment;

public class CourseService : ICoursesService
{
    private readonly ICourseRepository _courseRepository;

    public CourseService(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    public async Task<Result<IEnumerable<CourseDto>>> GetCoursesAsync()
    {
        var courses = await _courseRepository.GetAllWithDetailsAsync();

        var dto = courses.Select(c => new CourseDto
        {
            Id = c.CourseId,
            Title = c.Title,
            Subject = c.Subject,
            GradeLevel = c.GradeLevel.ToString(),
            Description = c.Description,
            TeacherName = c.Teacher != null
                ? $"{c.Teacher.FirstName} {c.Teacher.LastName}"
                : "Unknown Teacher",
            Sessions = c.Sessions.Select(s => new SessionDto
            {
                SessionId = s.SessionId,
                Title = s.Title,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                StreamUrl = s.StreamUrl
            }).ToList()
        });

        return Result<IEnumerable<CourseDto>>.Success(dto);
    }

    public async Task<Result<CourseDto>> GetCourseByIdAsync(int id)
    {
        var course = await _courseRepository.GetByIdWithDetailsAsync(id);
        if (course == null)
            return Result<CourseDto>.Failure("Course not found");

        var dto = new CourseDto
        {
            Id = course.CourseId,
            Title = course.Title,
            Subject = course.Subject,
            GradeLevel = course.GradeLevel.ToString(),
            Description = course.Description,
            TeacherName = course.Teacher != null
                ? $"{course.Teacher.FirstName} {course.Teacher.LastName}"
                : "Unknown Teacher",
            Sessions = course.Sessions.Select(s => new SessionDto
            {
                SessionId = s.SessionId,
                Title = s.Title,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                StreamUrl = s.StreamUrl
            }).ToList()
        };

        return Result<CourseDto>.Success(dto);
    }

    public async Task<Result<IEnumerable<CourseDto>>> GetByGradeAndSubjectAsync(CourseFilterDto filter)
    {
        var courses = await _courseRepository.GetByGradeAndSubjectAsync(filter);

        var dto = courses.Select(c => new CourseDto
        {
            Id = c.CourseId,
            Title = c.Title,
            Subject = c.Subject,
            GradeLevel = c.GradeLevel.ToString(),
            Description = c.Description,
            TeacherName = c.Teacher != null
                ? $"{c.Teacher.FirstName} {c.Teacher.LastName}"
                : "Unknown Teacher",
            Sessions = c.Sessions.Select(s => new SessionDto
            {
                SessionId = s.SessionId,
                Title = s.Title,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                StreamUrl = s.StreamUrl
            }).ToList()
        });

        return Result<IEnumerable<CourseDto>>.Success(dto);
    }
}


