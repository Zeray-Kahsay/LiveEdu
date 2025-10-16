using API.DTOs.Courses;
using API.Extensions;
using API.Helpers;
using API.Interfaces.Courses;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class CoursesController(ICoursesService coursesService) : BaseController
{
    [HttpGet("getCourses")]
    public async Task<ActionResult> GetCourses([FromQuery] CourseParams courseParams)
    {
        var result = await coursesService.GetCoursesAsync(courseParams);

        if (!result.IsSuccess)
            return BadRequest(result.Errors);

        return Ok(result.Value);

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
    public async Task<IActionResult> FilterCoursesByGradeAndSubject([FromQuery] CourseFilterDto filter)
    {
        var result = await coursesService.GetByGradeAndSubjectAsync(filter);

        if (result.IsSuccess) return Ok(result.Value);

        return NotFound(new ApiErrorDto
        {
            Status = 404,
            Message = "No courses found",
            Errors = result.Errors

        });
    }

    [HttpPost("add-course")]
    public async Task<IActionResult> AddCourse([FromBody] CourseCreateDto createCourseDto)
    {
        var teacherId = User.GetUserId();


        var result = await coursesService.AddCourseAsync(createCourseDto, teacherId);

        if (result.IsSuccess) return Ok(result.Value);

        return BadRequest(new ApiErrorDto
        {
            Status = 400,
            Message = "Could not add course",
            Errors = result.Errors
        });
    }

}
