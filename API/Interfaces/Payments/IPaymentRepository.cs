using API.Entities.Carts;

namespace API.Interfaces.Payments;

public interface IPaymentRepository
{
    Task<Payment?> GetPaymentByPaymentIntentIdAsync(string paymentIntentId);
    Task AddPaymentAsync(Payment payment);
    Task<Payment?> GetPaymentByIdAsync(int paymentId);
    Task<bool> SaveChangesAsync();

}
