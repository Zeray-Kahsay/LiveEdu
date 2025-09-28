using System;
using API.Entities;

namespace API.Helpers;

public class CourseParams : PaginationParams
{
    public string SearchTerm { get; set; } = string.Empty;
    public GradeLevel GradeLevel { get; set; }
    public string Subject { get; set; } = string.Empty;
}
