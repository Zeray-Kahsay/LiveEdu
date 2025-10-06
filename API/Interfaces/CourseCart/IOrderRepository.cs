using API.Entities.CourseCart;

namespace API.Interfaces.CourseCart;

public interface IOrderRepository
{
    Task<Order?> GetByPaymentIntentIdAsync(string paymentIntentId);
    Task AddOrderAsync(Order order);
    void UpdateOrder(Order order);
    Task<bool> SaveChangesAsync();
}
