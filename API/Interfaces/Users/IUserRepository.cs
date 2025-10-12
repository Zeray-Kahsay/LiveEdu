using API.Entities;
using API.Entities.Users;

namespace API.Interfaces.Users;

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
