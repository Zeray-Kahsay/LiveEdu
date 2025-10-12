namespace API.Entities.Carts;

public class Payment
{
    public int PaymentId { get; set; }
    public string PaymentIntentId { get; set; } = string.Empty;
    public string? Provider { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; }

    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
}

public enum PaymentStatus
{
    Pending = 0,
    Succeeded = 1,
    Failed = 2,
}
