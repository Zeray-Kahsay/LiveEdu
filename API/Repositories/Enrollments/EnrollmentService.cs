using API.DTOs.Enrollments;
using API.DTOs.Sessions;
using API.Entities;
using API.Entities.Courses;
using API.Helpers;
using API.Interfaces.Courses;
using API.Interfaces.Enrollments;

namespace API.Repositories.CourseEnrollment;

public class EnrollmentService(
    IEnrollmentRepository enrollmentRepository,
    ICourseRepository courseRepository
    ) : IEnrollmentService
{

    public async Task<Result<EnrollmentDto>> EnrollAsync(EnrollRequestDto enrollRequestDto)
    {
        //Check if course exists
        var course = await courseRepository.GetByIdAsync(enrollRequestDto.CourseId);
        if (course is null) return Result<EnrollmentDto>.Failure("Course not found");

        //Check if already enrolled
        var existing = await enrollmentRepository.GetEnrollmentByCourseAndStudentAsync(enrollRequestDto.CourseId, enrollRequestDto.StudentId);
        if (existing is not null) return Result<EnrollmentDto>.Failure("Already enrolled");

        var enrollment = new Enrollment
        {
            CourseId = enrollRequestDto.CourseId,
            StudentId = enrollRequestDto.StudentId,
            EnrolledAt = DateTime.UtcNow,
            Status = EnrollmentStatus.Enrolled
        };

        await enrollmentRepository.AddEnrollmentAsync(enrollment);

        var dto = new EnrollmentDto
        {
            EnrollmentId = enrollment.EnrollmentId,
            CourseId = course.CourseId,
            CourseTitle = course.Title,
            EnrolledAt = enrollment.EnrolledAt,
            Status = enrollment.Status.ToString()
        };

        return Result<EnrollmentDto>.Success(dto);

    }


    public async Task<Result<IEnumerable<EnrollmentDto>>> GetEnrollmentByStudentAsync(int studentId)
    {
        var enrollments = await enrollmentRepository.GetEnrollmentByStudentAsync(studentId);

        if (!enrollments.Any())
            return Result<IEnumerable<EnrollmentDto>>.Failure("No enrollment found");

        var dto = enrollments.Select(e => new EnrollmentDto
        {
            EnrollmentId = e.EnrollmentId,
            CourseId = e.CourseId,
            CourseTitle = e.Course.Title,
            EnrolledAt = e.EnrolledAt,
            Status = e.Status.ToString(),
            Subject = e.Course.Subject,
            GradeLevel = e.Course.GradeLevel.ToString(),
            TeacherName = $"{e.Course.Teacher.FirstName} {e.Course.Teacher.LastName}",

        });

        return Result<IEnumerable<EnrollmentDto>>.Success(dto);
    }


    public async Task<Result<EnrollmentDto>> GetEnrollmentByCourseAndStudentAsync(int courseId, int studentId)
    {
        var enrollment = await enrollmentRepository.GetEnrollmentByCourseAndStudentAsync(courseId, studentId);

        if (enrollment is null)
            return Result<EnrollmentDto>.Failure("No enrollment found");

        var dto = new EnrollmentDto
        {
            EnrollmentId = enrollment.EnrollmentId,
            CourseId = enrollment.CourseId,

            Description = enrollment.Course.Description,
            CourseTitle = enrollment.Course.Title,
            Subject = enrollment.Course.Subject,
            GradeLevel = enrollment.Course.GradeLevel.ToString(),
            TeacherName = $"{enrollment.Course.Teacher.FirstName} {enrollment.Course.Teacher.LastName}",
            Sessions = enrollment.Course.Sessions.Select(s => new SessionDto
            {
                SessionId = s.SessionId,
                Title = s.Title,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                StreamUrl = s.StreamUrl,
                IsLive = s.IsLive
            }).ToList()
        };

        return Result<EnrollmentDto>.Success(dto);
    }
}
