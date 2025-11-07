using API.DTOs.Carts;
using API.Entities.Carts;
using API.Helpers;

namespace API.Interfaces.Carts;

public interface ICartService
{
    Task<Result<Cart>> GetCartAsync(int id);
    Task<Result<CartDto>> AddItemAsync(int courseId, string? cartId, int userId);
    Task<Result<CartDto>> MergeCartsAsync(string guestCartId, int userId);
    //Task<Result<Cart>> UpdateCartAsync(Cart cart);
    Task<Result<CartDto>> RemoveItemAsync(string id, int courseId);
    Task<Result<CartDto>> ClearCartAsync(string id);
}
