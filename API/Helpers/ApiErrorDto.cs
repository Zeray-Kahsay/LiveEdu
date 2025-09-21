namespace API.Helpers;

public class ApiErrorDto
{
    public int Status { get; set; }
    public string Message { get; set; } = default!;
    public IEnumerable<string>? Errors { get; set; }
}
