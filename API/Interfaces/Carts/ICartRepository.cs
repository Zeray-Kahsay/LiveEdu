using API.Entities.Carts;

namespace API.Interfaces.Carts;

public interface ICartRepository
{
    Task<Cart?> GetCartByIdAsync(int id);
    void UpdateCart(Cart cart);
    Task AddCartAsync(Cart cart);
    Task<bool> SaveChangesAsync();

}
