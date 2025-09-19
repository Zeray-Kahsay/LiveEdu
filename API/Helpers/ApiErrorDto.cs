namespace API.Helpers;

public class ApiErrorDto
{
    public int StatusCode { get; set; }
    public string Message { get; set; } = default!;
    public IEnumerable<string>? Errors { get; set; }
}
