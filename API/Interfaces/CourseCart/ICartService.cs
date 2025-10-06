using API.Entities.CourseCart;
using API.Helpers;

namespace API.Interfaces.CourseCart;

public interface ICartService
{
    Task<Result<Cart>> GetCartAsync(string cartId);
    Task<Result<Cart>> AddItemAsync(string cartId, int courseId);
    //Task<Result<Cart>> UpdateCartAsync(Cart cart);
    Task<Result<Cart>> RemoveItemAsync(string cartId, int courseId);
    Task<Result> ClearCartAsync(string cartId);
}
