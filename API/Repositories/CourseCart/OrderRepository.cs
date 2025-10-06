using API.Data;
using API.Entities.CourseCart;
using API.Interfaces.CourseCart;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.CourseCart;

public class OrderRepository(AppDbContext context) : IOrderRepository
{
    public async Task<Order?> GetByPaymentIntentIdAsync(string paymentIntentId)
    {
        return await context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.PaymentIntentId == paymentIntentId);
    }


    public async Task AddOrderAsync(Order order) => await context.Orders.AddAsync(order);


    public void UpdateOrder(Order order) => context.Orders.Update(order);


    public async Task<bool> SaveChangesAsync() => await context.SaveChangesAsync() > 0;


}
