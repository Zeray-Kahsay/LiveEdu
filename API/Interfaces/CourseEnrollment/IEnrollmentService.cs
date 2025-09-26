using System;
using API.DTOs.Course;
using API.Helpers;

namespace API.Interfaces.CourseEnrollment;

public interface IEnrollmentService
{
    Task<Result<EnrollmentDto>> EnrollAsync(EnrollRequestDto enrollRequestDto);
    Task<Result<IEnumerable<EnrollmentDto>>> GetEnrollmentByStudentAsync(int studentId);
    Task<Result<EnrollmentDto>> GetEnrollmentByCourseAndStudentAsync(int courseId, int studentId);

}
