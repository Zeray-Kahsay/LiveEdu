using API.Data;
using API.DTOs.Account;
using API.DTOs.Account.User;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using API.Interfaces.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.Account;

public class AccountRepository(UserManager<AppUser> userManager,
       SignInManager<AppUser> signInManager,
       ITokenService tokenService,
       AppDbContext context,
       ILogger<AccountRepository> logger
       ) : IAccountRepository
{
    public async Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto)
    {
        try
        {

            var existsResult = await UserExistsAsync(registerDto.Email);
            if (!existsResult.IsSuccess)
                return Result<UserDto>.Failure(existsResult.Errors!);



            var user = new AppUser
            {
                UserName = registerDto.Email,
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

            // Ensure role exists
            if (!await userManager.IsInRoleAsync(user, registerDto.Role))
            {
                var roleExists = await userManager.AddToRoleAsync(user, registerDto.Role);
                if (!roleExists.Succeeded)
                {
                    var errors = roleExists.Errors.Select(e => e.Description).ToArray();
                    return Result<UserDto>.Failure(errors);
                }
            }
            await userManager.AddToRoleAsync(user, registerDto.Role);

            //var token = await tokenService.CreateToken(user);

            var userDto = new UserDto
            {
                Id = user.Id,
                // Username = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                SchoolName = registerDto.SchoolName,
            };

            return Result<UserDto>.Success(userDto);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred during user registration");
            return Result<UserDto>.Failure("An Internal error occurred during registration. Please try again later.");
        }


    }


    public async Task<Result<AuthResponseDto>> LoginAsync(LoginDto loginDto)
    {
        try
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
                return Result<AuthResponseDto>.Failure("Invalid email or password");

            var result = await signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
            if (!result.Succeeded)
                return Result<AuthResponseDto>.Failure("Invalid email or password");

            var accessToken = await tokenService.CreateToken(user, loginDto.DeviceId);
            var refreshToken = await tokenService.CreateRefreshToken(user, loginDto.DeviceId);

            var response = new AuthResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken.Token,
                User = new UserDto
                {
                    Id = user.Id,
                    //Username = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    SchoolName = user.SchoolName,
                }
            };



            return Result<AuthResponseDto>.Success(response);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred during user login");
            return Result<AuthResponseDto>.Failure("An Internal error occurred during login. Please try again later.");
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
            if (await userManager.FindByEmailAsync(email) != null)
                return Result<bool>.Failure("Email is already taken");

            return Result<bool>.Success(false);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error occurred while checking if user exists");
            return Result<bool>.Failure("An internal error occurred. Please try again later.");
        }

    }

    public async Task<Result<AuthResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto dto)
    {
        try
        {
            // lookup refresh token in DB
            var storedToken = await context.RefreshTokens
                                .Include(rt => rt.User)
                                .FirstOrDefaultAsync(rt => rt.Token == dto.RefreshToken && rt.DeviceId == dto.DeviceId);

            if (storedToken is null || storedToken.IsRevoked || storedToken.Expires < DateTime.UtcNow)
                return Result<AuthResponseDto>.Failure("Invalid or expired refresh token");


            var appUser = storedToken.User;
            if (appUser is null)
                return Result<AuthResponseDto>.Failure("User not found");

            // Generate new tokens
            var newAccessToken = await tokenService.CreateToken(appUser, dto.DeviceId);
            var newRefreshToken = await tokenService.CreateRefreshToken(appUser, dto.DeviceId);

            // Revoke the old token
            storedToken.IsRevoked = true;
            await context.SaveChangesAsync();

            var response = new AuthResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token,
                User = new UserDto
                {
                    Id = appUser.Id,
                    Email = appUser.Email,
                    FirstName = appUser.FirstName,
                    LastName = appUser.LastName,
                    SchoolName = appUser.SchoolName
                }
            };

            return Result<AuthResponseDto>.Success(response);

        }
        catch (Exception ex)
        {

            logger.LogError(ex, "Error refreshing token");
            return Result<AuthResponseDto>.Failure("An internal error occurred while refreshing token");
        }


    }

}
