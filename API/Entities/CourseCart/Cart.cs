
namespace API.Entities.CourseCart;

public class Cart
{
    public string CartId { get; set; } = Guid.NewGuid().ToString();
    public int? UserId { get; set; } // optional for guest users 
    public ICollection<CartItem> Items { get; set; } = [];

}
