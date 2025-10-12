using API.DTOs.Enrollments;
using API.Helpers;
using API.Interfaces.Enrollments;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize]
public class EnrollmentsController(IEnrollmentService enrollmentService) : BaseController
{
    [HttpPost("enrollCourse")]
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

    [HttpGet("student/{studentId}/course/{courseId}")]
    public async Task<IActionResult> GetEnrollmentByCourseAndStudent(int studentId, int courseId)
    {
        var result = await enrollmentService.GetEnrollmentByCourseAndStudentAsync(courseId, studentId);

        if (result.IsSuccess) return Ok(result.Value);

        return NotFound(new ApiErrorDto
        {
            Status = 404,
            Message = "Not enrolled in this course",
            Errors = result.Errors
        });
    }

}
