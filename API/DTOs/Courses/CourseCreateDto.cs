
using System.ComponentModel.DataAnnotations;
using API.Entities.Courses;

namespace API.DTOs.Courses;

public record CourseCreateDto
{
    [Required]
    public string Title { get; set; } = string.Empty;
    [Required]
    public string Description { get; set; } = string.Empty;
    [Required]
    public decimal Price { get; set; }
    [Required]
    public string ImageUrl { get; set; } = string.Empty;
    [Required]
    public DateTime StartDate { get; set; }
    [Required]
    public DateTime EndDate { get; set; }
    [Required]
    public int MaxStudents { get; set; }
    [Required]
    public GradeLevel GradeLevel { get; set; }
    [Required]
    public string Subject { get; set; } = string.Empty;
}
