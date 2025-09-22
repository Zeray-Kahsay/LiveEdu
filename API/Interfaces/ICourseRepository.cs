using System;
using API.Entities;

namespace API.Interfaces;

public interface ICourseRepository : IRepository<Course>
{
    Task<IEnumerable<Course>> GetByGradeAndSubjectAsync(GradeLevel gradeLevel, string subject);
    Task<IEnumerable<Course>> GetAllWithDetailsAsync();
    Task<Course?> GetByIdWithDetailsAsync(int id);
}
