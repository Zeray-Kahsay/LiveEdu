namespace API.Entities.CourseCart;

public class Order
{
    public int OrderId { get; set; }
    public int? UserId { get; set; }
    public AppUser? User { get; set; } // ???
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public decimal Total { get; set; } // ??? 
    public bool IsPaid { get; set; }
    public DateTime PaidAt { get; set; } = DateTime.UtcNow;
    public string PaymentIntentId { get; set; } = string.Empty;
    public ICollection<OrderItem> Items { get; set; } = [];
    public string PaymentStatus { get; set; } = string.Empty;
}

// public enum PaymentStatus
// {
//     Pending = 0,
//     Succeeded = 1,
//     Failed = 2,
// }
