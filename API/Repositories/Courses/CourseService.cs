using API.DTOs.Courses;
using API.DTOs.Sessions;
using API.Entities.Courses;
using API.Helpers;
using API.Interfaces.Courses;

namespace API.Repositories.CourseEnrollment;



public class CourseService : ICoursesService
{
    private readonly ICourseRepository _courseRepository;

    public CourseService(ICourseRepository courseRepository)
    {
        _courseRepository = courseRepository;
    }

    // public async Task<Result<IEnumerable<CourseDto>>> GetCoursesAsync()
    // {
    //     var courses = await _courseRepository.GetAllWithDetailsAsync();

    //     var dto = courses.Select(c => new CourseDto
    //     {
    //         Id = c.CourseId,
    //         Title = c.Title,
    //         Subject = c.Subject,
    //         GradeLevel = c.GradeLevel.ToString(),
    //         Description = c.Description,
    //         TeacherName = c.Teacher != null
    //             ? $"{c.Teacher.FirstName} {c.Teacher.LastName}"
    //             : "Unknown Teacher",
    //         Sessions = c.Sessions.Select(s => new SessionDto
    //         {
    //             SessionId = s.SessionId,
    //             Title = s.Title,
    //             StartTime = s.StartTime,
    //             EndTime = s.EndTime,
    //             StreamUrl = s.StreamUrl
    //         }).ToList()
    //     });

    //     return Result<IEnumerable<CourseDto>>.Success(dto);
    // }

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
            Price = course.Price,
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
            Price = c.Price,
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

    public async Task<Result<CoursePageDto>> GetCoursesAsync(CourseParams courseParams)
    {
        var courses = await _courseRepository.GetCoursesAsync(courseParams);

        var dtoList = courses.Select(c => new CourseDto
        {
            Id = c.CourseId,
            Title = c.Title,
            Subject = c.Subject,
            GradeLevel = c.GradeLevel.ToString(),
            Description = c.Description,
            Price = c.Price,
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
        }).ToList();

        var hasNextPage = courses.Count > courseParams.PageSize;
        var lastId = dtoList.LastOrDefault()?.Id;

        var response = new CoursePageDto
        {
            Courses = dtoList,
            HasNextPage = hasNextPage,
            LastId = lastId
        };

        return Result<CoursePageDto>.Success(response);
    }

    public async Task<Result<Course>> AddCourseAsync(CourseCreateDto dto, int teacherId)
    {
        var course = new Course
        {
            Title = dto.Title,
            Description = dto.Description,
            Price = dto.Price,
            ImageUrl = dto.ImageUrl,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            MaxStudents = dto.MaxStudents,
            GradeLevel = dto.GradeLevel,
            Subject = dto.Subject,
            TeacherId = teacherId
        };

        await _courseRepository.AddCourseAsync(course);
        var success = await _courseRepository.SaveAllAsync();
        if (!success) return Result<Course>.Failure("Failed to add course");

        return Result<Course>.Success(course);
    }
}


