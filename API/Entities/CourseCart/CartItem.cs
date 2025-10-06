namespace API.Entities.CourseCart;

public class CartItem
{
    public int Id { get; set; }

    // FK and navigation to the course
    public int CourseId { get; set; }
    public Course Course { get; set; } = default!;

    // Snapshot fields so i can freez data at time of adding
    public string CourseTitle { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string GradeLevel { get; set; } = string.Empty;

    // Cart reference 
    public string CartId { get; set; } = default!;
    public Cart Cart { get; set; } = default!;

}