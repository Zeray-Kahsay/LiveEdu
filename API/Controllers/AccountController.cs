using API.DTOs.Account;
using API.Helpers;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;


public class AccountController(IAccountRepository accountRepository) : BaseController
{
    [HttpPost("registerUser")]
    public async Task<IActionResult> RegisterUser([FromBody] RegisterDto registerDto)
    {
        var result = await accountRepository.RegisterAsync(registerDto);
        if (result.IsSuccess)
        {
            // Return 201 Created with the user data
            return StatusCode(StatusCodes.Status201Created, result.Value);
        }

        var errors = result.Errors?.ToArray() ?? new[] { "An unknown error occurred." };
        var status = errors.Any(e => e.Equals("Email is already taken", StringComparison.OrdinalIgnoreCase) ||
                                     e.Equals("Username is already taken", StringComparison.OrdinalIgnoreCase))
            ? StatusCodes.Status400BadRequest
            : StatusCodes.Status500InternalServerError;

        var errorResponse = new ApiErrorDto
        {
            Status = status,
            Message = "Registration failed",
            Errors = errors
        };
        return StatusCode(status, errorResponse);

    }

    [HttpPost("loginUser")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        var result = await accountRepository.LoginAsync(loginDto);
        if (result.IsSuccess)
        {
            return StatusCode(StatusCodes.Status200OK, result.Value);
        }

        var errors = result.Errors?.ToArray() ?? new[] { "An unknown error occurred." };
        var status = errors.Any(e => e.Equals("Invalid email or password", StringComparison.OrdinalIgnoreCase))
            ? StatusCodes.Status401Unauthorized
            : StatusCodes.Status500InternalServerError;
        Console.WriteLine(errors);

        var errorResponse = new ApiErrorDto
        {
            Status = status,
            Message = "Login failed",
            Errors = errors
        };
        return StatusCode(status, errorResponse);
    }

}
