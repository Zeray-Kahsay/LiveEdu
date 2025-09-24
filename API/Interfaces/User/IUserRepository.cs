using API.Entities;

namespace API.Interfaces.User;

public interface IUserRepository
{
    Task<AppUser?> GetUserByIdAsync(int id);
    Task<AppUser?> GetUserByUsernameAsync(string username);
    Task<AppUser?> GetUserByEmailAsync(string email);
    Task<AppUser?> GetCurrentUserAsync();
    Task<IEnumerable<AppUser>> GetUsersAsync();

    Task UpdateUserAsync(AppUser user);
    Task DeleteUserAsync(int id);
    Task<bool> SaveAllAsync();


}
