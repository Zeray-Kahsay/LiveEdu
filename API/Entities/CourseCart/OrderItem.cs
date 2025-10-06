namespace API.Entities.CourseCart;

public class OrderItem
{
    public int OrderItemId { get; set; }
    public int OrderId { get; set; }
    public Order Order { get; set; } = null!;
    public int CourseId { get; set; }
    public string CourseTitle { get; set; } = string.Empty;
    //public required Course Course { get; set; } // why excluded ?? 
    public decimal Price { get; set; } // why included since we can get it from course price 
    public int Quantity { get; set; } = 1;
}
