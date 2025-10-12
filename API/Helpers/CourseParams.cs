using API.Entities;
using API.Entities.Courses;

namespace API.Helpers;

public class CourseParams
{
    public string? SearchTerm { get; set; }
    public GradeLevel? GradeLevel { get; set; }
    public string? Subject { get; set; }
    public int PageSize { get; set; } = 6;
    public int? LastId { get; set; } // for Keyset pagination

}
