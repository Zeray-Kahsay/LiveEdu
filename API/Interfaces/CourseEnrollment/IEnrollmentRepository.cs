using API.Entities;

namespace API.Interfaces.CourseEnrollment;

public interface IEnrollmentRepository
{
    Task<IEnumerable<Enrollment>> GetEnrollmentByStudentAsync(int StudentId);
    Task<Enrollment?> GetEnrollmentByCourseAndStudentAsync(int courseId, int studentId);
    Task AddEnrollmentAsync(Enrollment enrollment);

}
