using API.Entities.Carts;

namespace API.Interfaces.Carts;

public interface ICartRepository
{
    Task<Cart?> GetCartByIdAsync(string cartId);
    void UpdateCart(Cart cart);
    Task AddCartAsync(Cart cart);
    Task<bool> SaveChangesAsync();

}
