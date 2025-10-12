using API.Entities.Carts;
using API.Helpers;

namespace API.Interfaces.Carts;

public interface ICartService
{
    Task<Result<Cart>> GetCartAsync(string cartId);
    Task<Result<Cart>> AddItemAsync(string cartId, int courseId);
    //Task<Result<Cart>> UpdateCartAsync(Cart cart);
    Task<Result<Cart>> RemoveItemAsync(string cartId, int courseId);
    Task<Result> ClearCartAsync(string cartId);
}
