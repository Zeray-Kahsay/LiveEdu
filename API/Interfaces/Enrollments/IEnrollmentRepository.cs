using API.Entities;
using API.Entities.Courses;

namespace API.Interfaces.Enrollments;

public interface IEnrollmentRepository
{
    Task<IEnumerable<Enrollment>> GetEnrollmentByStudentAsync(int StudentId);
    Task<Enrollment?> GetEnrollmentByCourseAndStudentAsync(int courseId, int studentId);
    Task AddEnrollmentAsync(Enrollment enrollment);

}
