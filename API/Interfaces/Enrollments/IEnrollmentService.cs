using API.DTOs.Enrollments;
using API.Helpers;

namespace API.Interfaces.Enrollments;

public interface IEnrollmentService
{
    Task<Result<EnrollmentDto>> EnrollAsync(EnrollRequestDto enrollRequestDto);
    Task<Result<IEnumerable<EnrollmentDto>>> GetEnrollmentByStudentAsync(int studentId);
    Task<Result<EnrollmentDto>> GetEnrollmentByCourseAndStudentAsync(int courseId, int studentId);

}
