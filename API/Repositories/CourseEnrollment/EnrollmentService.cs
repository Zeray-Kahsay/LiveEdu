using API.DTOs.Course;
using API.Entities;
using API.Helpers;
using API.Interfaces.CourseEnrollment;

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

        var dto = enrollments.Select(e => new EnrollmentDto
        {
            EnrollmentId = e.EnrollmentId,
            CourseId = e.CourseId,
            CourseTitle = e.Course.Title,
            EnrolledAt = e.EnrolledAt,
            Status = e.Status.ToString()
        });

        return Result<IEnumerable<EnrollmentDto>>.Success(dto);
    }


}
