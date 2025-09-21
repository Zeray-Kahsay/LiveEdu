using System;

namespace API.Entities;

public class Submission
{
    public int SubmissionId { get; set; }

    public int AssignmentId { get; set; }
    public Assignment Assignment { get; set; } = null!;

    public int StudentId { get; set; }
    public AppUser Student { get; set; } = null!;
    public string Content { get; set; } = string.Empty;
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    public double? Grade { get; set; }
    public string? Feedback { get; set; }

    

}
