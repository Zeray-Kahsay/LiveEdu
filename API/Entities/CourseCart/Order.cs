namespace API.Entities.CourseCart;

public class Order
{
    public int OrderId { get; set; }
    public int? UserId { get; set; }
    public AppUser? User { get; set; } // ???
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public decimal Total { get; set; } // ??? 
    public bool IsPaid { get; set; }
    public string PaymentIntentId { get; set; } = string.Empty;
    public ICollection<OrderItem> Items { get; set; } = [];
}
