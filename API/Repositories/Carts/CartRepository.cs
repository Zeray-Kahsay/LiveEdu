using API.Data;
using API.Entities.Carts;
using API.Interfaces.Carts;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.Carts;

public class CartRepository(AppDbContext context) : ICartRepository
{
    public async Task<Cart?> GetCartByIdAsync(int id)
    {
        return await context.Carts
                    .Include(c => c.Items)
                    .ThenInclude(ct => ct.Course)
                    .FirstOrDefaultAsync(c => c.Id == id);
    }


    public async Task AddCartAsync(Cart cart)
    {
        await context.Carts.AddAsync(cart);
    }


    public void UpdateCart(Cart cart)
    {
        context.Carts.Update(cart);
    }


    public async Task<bool> SaveChangesAsync()
    {
        return await context.SaveChangesAsync() > 0;
    }
}


