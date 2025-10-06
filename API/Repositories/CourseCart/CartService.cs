using API.Data;
using API.Entities;
using API.Entities.CourseCart;
using API.Helpers;
using API.Interfaces.CourseCart;

namespace API.Repositories.CourseCart;

public class CartService(
    ICartRepository cartRepository,
    ILogger<CartService> logger,
    AppDbContext context
    ) : ICartService
{
    public async Task<Result<Cart>> GetCartAsync(string cartId)
    {
        try
        {
            var cart = await cartRepository.GetCartByIdAsync(cartId);
            if (cart is null) return Result<Cart>.Failure("Cart not found");

            return Result<Cart>.Success(cart);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching card {id}", cartId);
            return Result<Cart>.Failure("An error occured while fetching the cart ");
        }
    }

    public async Task<Result<Cart>> AddItemAsync(string cartId, int courseId)
    {
        try
        {
            // Get the cart 
            var cart = await cartRepository.GetCartByIdAsync(cartId);

            // if no cart, create and add it to Carts
            if (cart is null)
            {
                cart = new Cart { CartId = cartId };
                await cartRepository.AddCartAsync(cart);
            }

            // Get course 
            var course = await context.Courses.FindAsync(courseId);
            if (course is null) return Result<Cart>.Failure("Course not found");

            // check if the course/item already exists in the cart 
            if (cart.Items.Any(i => i.CourseId == courseId))
                return Result<Cart>.Failure("Course already in cart");

            // if not, create a cartItem and add it onto the cart
            var item = new CartItem
            {
                CourseId = course.CourseId,
                CourseTitle = course.Title,
                Price = course.Price,
                ImageUrl = course.ImageUrl ?? string.Empty,
                Subject = course.Subject ?? string.Empty,
                GradeLevel = course.GradeLevel.ToString(),
                CartId = cart.CartId
            };

            // add the item 
            cart.Items.Add(item);


            // Persist to DB
            if (await cartRepository.SaveChangesAsync())
                return Result<Cart>.Success(cart);

            return Result<Cart>.Failure("Failed to add item to cart");
        }
        catch (Exception ex)
        {
            logger.LogError($"Error adding item to cart with cart id: {cartId} and course id:  {courseId}", ex);
            return Result<Cart>.Failure("An error occurred while adding item to cart");
        }

    }

    public async Task<Result> ClearCartAsync(string cartId)
    {
        try
        {
            // Get the cart 
            var cart = await cartRepository.GetCartByIdAsync(cartId);
            if (cart is null) return Result.Failure("Cart not found");

            // if found clear the items
            context.CartItems.RemoveRange(cart.Items);

            if (await cartRepository.SaveChangesAsync())
                return Result.Success();

            return Result.Failure("Failed to clear cart");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error clearing cart {id}", cartId);
            return Result.Failure("An error occurred while clearing the cart");
        }
    }

    public async Task<Result<Cart>> RemoveItemAsync(string cartId, int courseId)
    {
        try
        {
            // Get the cart 
            var cart = await cartRepository.GetCartByIdAsync(cartId);
            if (cart is null) return Result<Cart>.Failure("Cart not found");

            // Get the item from the cart - and then remove it
            var item = cart.Items.FirstOrDefault(i => i.CourseId == courseId);
            if (item is null) return Result<Cart>.Failure("Item not found in cart");

            cart.Items.Remove(item);

            // Persist the change in DB
            if (await cartRepository.SaveChangesAsync())
                return Result<Cart>.Success(cart);

            return Result<Cart>.Failure("Failed to remove item");
        }
        catch (Exception ex)
        {
            logger.LogError($"Error removing an item from cart id: {cartId} and course id: {courseId}", ex);
            return Result<Cart>.Failure("An error occurred while removing item from cart");
        }
    }
}
