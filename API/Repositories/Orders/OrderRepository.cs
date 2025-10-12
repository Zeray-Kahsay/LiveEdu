using API.Data;
using API.Entities.Carts;
using API.Interfaces.Orders;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.CourseCart;

public class OrderRepository(AppDbContext context) : IOrderRepository
{
    public async Task AddOrderAsync(Order order) => await context.Orders.AddAsync(order);


    public async Task<Order?> GetOrderByIdAsync(int orderId) =>
        await context.Orders
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Course)
            .FirstOrDefaultAsync(o => o.OrderId == orderId);

    public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(int userId)
    {
        return await context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.Items)
            .ThenInclude(oi => oi.Course)
            .ToListAsync();
    }

    public async Task<bool> SaveChangesAsync() =>
        await context.SaveChangesAsync() > 0;

    public async Task UpdateOrder(Order order)
    {
        context.Orders.Update(order);
        await context.SaveChangesAsync();
    }
}
