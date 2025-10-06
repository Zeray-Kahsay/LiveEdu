
using API.Entities.CourseCart;
using API.Helpers;

namespace API.Interfaces.CourseCart;

public interface ICartRepository
{
    Task<Cart?> GetCartByIdAsync(string cartId);
    void UpdateCart(Cart cart);
    Task AddCartAsync(Cart cart);
    Task<bool> SaveChangesAsync();

}
