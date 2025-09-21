using API.DTOs.Account;
using API.DTOs.Account.User;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Identity;

namespace API.Repositories;

public class AccountRepository(UserManager<AppUser> userManager,
       SignInManager<AppUser> signInManager,
       //AppDbContext context,
       ITokenService tokenService,
       ILogger<AccountRepository> logger
       ) : IAccountRepository
{
    public async Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto)
    {
        try
        {

            if (await userManager.FindByEmailAsync(registerDto.Email) != null)
                return Result<UserDto>.Failure("Email is already taken");

            if (await userManager.FindByNameAsync(registerDto.Username) != null)
                return Result<UserDto>.Failure("Username is already taken");

            var user = new AppUser
            {
                UserName = registerDto.Username,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                CreatedAt = DateTime.UtcNow
            };

            var result = await userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description).ToArray();
                return Result<UserDto>.Failure(errors);
            }


            await userManager.AddToRoleAsync(user, registerDto.Role);

            var token = await tokenService.CreateToken(user);

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = token
            };

            return Result<UserDto>.Success(userDto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred during user registration");
            return Result<UserDto>.Failure("An Internal error occurred during registration. Please try again later.");
        }


    }


    public async Task<Result<UserDto>> LoginAsync(LoginDto loginDto)
    {
        try
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                return Result<UserDto>.Failure("Invalid email or password");

            var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
                return Result<UserDto>.Failure("Invalid email or password");

            var token = await tokenService.CreateToken(user);

            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = token
            };

            return Result<UserDto>.Success(userDto);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred during user login");
            return Result<UserDto>.Failure("An Internal error occurred during login. Please try again later.");
        }

    }

    public Task<Result> ConfirmEmailAsync(ConfirmEmailDto confirmEmailDto)
    {
        throw new NotImplementedException();
    }

    public Task<Result<string>?> GenerateEmailConfirmationTokenAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<Result> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        throw new NotImplementedException();
    }

    public Task<Result> ChangePasswordAsync(ChangePasswordDto changePasswordDto)
    {
        throw new NotImplementedException();
    }

    public async Task<Result<bool>> UserExistsAsync(string email)
    {
        try
        {
            var user = await userManager.FindByEmailAsync(email);
            return Result<bool>.Success(user != null);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while checking if user exists");
            return Result<bool>.Failure("An internal error occurred. Please try again later.");
        }

    }
}
