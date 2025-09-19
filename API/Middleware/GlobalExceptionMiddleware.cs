using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using API.Helpers;

namespace API.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        _logger.LogError(ex, "Unhandled exception");

        var status = ex switch
        {
            UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
            ValidationException => StatusCodes.Status400BadRequest,
            ArgumentException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            _ => StatusCodes.Status500InternalServerError
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = status;

        var errors = new List<string> { ex.Message };

        // In development include details to help debugging
        if (_env.IsDevelopment())
        {
            if (ex.InnerException != null) errors.Add(ex.InnerException.Message);
            errors.Add(ex.StackTrace ?? string.Empty);
        }

        var dto = new ApiErrorDto
        {
            StatusCode = status,
            Message = status == StatusCodes.Status500InternalServerError
                ? "An unexpected error occurred."
                : "One or more errors occurred.",
            Errors = errors
        };

        var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
        await context.Response.WriteAsync(JsonSerializer.Serialize(dto, options));
    }

 
}