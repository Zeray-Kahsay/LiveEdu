using API.DTOs.Course;
using API.Helpers;
using API.Interfaces.CourseEnrollment;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class EnrollmentsController(IEnrollmentService enrollmentService) : BaseController
{
    [HttpPost]
    public async Task<IActionResult> Enroll([FromBody] EnrollRequestDto request)
    {
        var result = await enrollmentService.EnrollAsync(request);

        if (result.IsSuccess) return Ok(result.Value);

        return BadRequest(new ApiErrorDto
        {
            Status = 400,
            Message = "Enrollment failed",
            Errors = result.Errors
        });
    }

    [HttpGet("student/{studentId}")]
    public async Task<IActionResult> GetByStudent(int studentId)
    {
        var result = await enrollmentService.GetEnrollmentByStudentAsync(studentId);

        if (result.IsSuccess) return Ok(result.Value);

        return NotFound(new ApiErrorDto
        {
            Status = 404,
            Message = "No enrollments found",
            Errors = result.Errors
        });
    }
}
