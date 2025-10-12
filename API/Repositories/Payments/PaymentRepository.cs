using API.Data;
using API.Entities.Carts;
using API.Interfaces.Payments;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.Payments;

public class PaymentRepository(AppDbContext context) : IPaymentRepository
{

    public async Task AddPaymentAsync(Payment payment) => await context.Payments.AddAsync(payment);


    public async Task<bool> SaveChangesAsync() => await context.SaveChangesAsync() > 0;

    public async Task<Payment?> GetPaymentByPaymentIntentIdAsync(string paymentIntentId)
    {
        return await context.Payments
           .Include(p => p.Order)
           .FirstOrDefaultAsync(p => p.PaymentIntentId == paymentIntentId);
    }

    public async Task<Payment?> GetPaymentByIdAsync(int paymentId)
    {
        return await context.Payments
           .Include(p => p.Order)
           .FirstOrDefaultAsync(p => p.PaymentId == paymentId);
    }
}
