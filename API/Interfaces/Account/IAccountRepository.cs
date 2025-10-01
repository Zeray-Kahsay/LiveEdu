using API.DTOs.Account;
using API.DTOs.Account.User;
using API.Entities;
using API.Helpers;

namespace API.Interfaces.Account;

public interface IAccountRepository
{
    Task<Result<UserDto>> RegisterAsync(RegisterDto registerDto);
    Task<Result<AuthResponseDto>> LoginAsync(LoginDto loginDto);
    Task<Result> ConfirmEmailAsync(ConfirmEmailDto confirmEmailDto);
    Task<Result<string>?> GenerateEmailConfirmationTokenAsync(string email);
    Task<Result> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    Task<Result> ChangePasswordAsync(ChangePasswordDto changePasswordDto);
    Task<Result<AuthResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto refreshTokenRequestDto);
    Task<Result<bool>> UserExistsAsync(string email);


}
