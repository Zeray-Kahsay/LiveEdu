using API.Data.Migrations;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class CoursesController(ICoursesService coursesService) : BaseController
{
    [HttpGet]
    public async Task<IActionResult> GetCourses()
    {
        var result = await coursesService.GetCoursesAsync();

        if (result.IsSuccess) return Ok(result.Value);

        return StatusCode(500, new ApiErrorDto
        {
            Status = 500,
            Message = "Failed to fetch courses",
            Errors = result.Errors
        });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCourseById(int id)
    {
        var result = await coursesService.GetCourseByIdAsync(id);

        if (result.IsSuccess) return Ok(result.Value);

        return StatusCode(404, new ApiErrorDto
        {
            Status = 404,
            Message = "Course not found",
            Errors = result.Errors
        });
    }

    [HttpGet("filter")]
    public async Task<IActionResult> GetByGradeAndSubject([FromQuery] GradeLevel gradeLevel, [FromQuery] string subject)
    {
        var result = await coursesService.GetByGradeAndSubjectAsync(gradeLevel, subject);

        if (result.IsSuccess) return Ok(result.Value);

        return StatusCode(500, new ApiErrorDto
        {
            Status = 500,
            Message = "Failed to fetch filtered courses",
            Errors = result.Errors

        });
    }

}
