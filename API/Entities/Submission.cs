using System;

namespace API.Entities;

public class Submission
{
    public int SubmissionId { get; set; }
    public int AssignmentId { get; set; }
    public int StudentId { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTime SubmissionDate { get; set; }

}
