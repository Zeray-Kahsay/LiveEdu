using API.Entities.Users;

namespace API.Entities.Carts;

public class Order
{
    public int OrderId { get; set; }
    public int? UserId { get; set; }
    public AppUser? User { get; set; }
    public DateTime PaidAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public decimal Total { get; set; }
    public bool IsPaid { get; set; }


    public ICollection<OrderItem> Items { get; set; } = [];
    public Payment? Payment { get; set; }
    public int? PaymentId { get; set; }
}


