
using System.ComponentModel.DataAnnotations;

namespace API.Entities.Carts;

public class Cart
{
    public int Id { get; set; }
    public string CartId { get; set; } = Guid.NewGuid().ToString();
    public int? UserId { get; set; } // optional for guest users 
    public ICollection<CartItem> Items { get; set; } = [];
    public string? PaymentIntentId { get; set; }
    public string? ClientSecretId { get; set; }

    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;


}

public enum PaymentStatus
{
    Pending,
    succeeded,
    Failed
}
