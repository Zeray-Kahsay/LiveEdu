using API.Data;
using API.DTOs.Carts;
using API.Entities.Carts;
using API.Extensions.MappingExtensions;
using API.Helpers;
using API.Interfaces.Carts;
using Humanizer;
using Microsoft.EntityFrameworkCore;

namespace API.Repositories.Carts;

public class CartService(
    ICartRepository cartRepository,
    ILogger<CartService> logger,
    AppDbContext context
    ) : ICartService
{
    public async Task<Result<Cart>> GetCartAsync(int id)
    {
        try
        {
            var cart = await cartRepository.GetCartByIdAsync(id);
            if (cart is null) return Result<Cart>.Failure("Cart not found");

            return Result<Cart>.Success(cart);

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error fetching card {id}", id);
            return Result<Cart>.Failure("An error occured while fetching the cart ");
        }
    }

    public async Task<Result<CartDto>> AddItemAsync(int courseId, string? cartId, int userId)
    {
        try
        {
            // Get course 
            var course = await context.Courses.FindAsync(courseId);
            if (course is null) return Result<CartDto>.Failure("Course not found");

            Cart? cart;

            if (string.IsNullOrEmpty(cartId))
            {
                // create a new cart 
                cart = new Cart
                {
                    CartId = Guid.NewGuid().ToString(),
                    UserId = userId > 0 ? userId : null
                };

                // persist to DB
                await cartRepository.AddCartAsync(cart);
            }
            else
            {
                cart = await context.Carts
                    .Include(c => c.Items)
                    .ThenInclude(i => i.Course)
                    .FirstOrDefaultAsync(c => c.CartId == cartId);

                if (cart is null)
                {
                    cart = new Cart
                    {
                        CartId = cartId,
                        UserId = userId > 0 ? userId : null
                    };

                    await cartRepository.AddCartAsync(cart);

                }
                else
                {
                    if (userId > 0 && cart.UserId == null)
                    {
                        // Attach guest cart to logged-in user
                        cart.UserId = userId;
                    }

                }

                // Ensure the Items collection is initialized to avoid null dereference
                cart.Items = new List<CartItem>();
            }



            // check if the course/item already exists in the cart 
            if (cart.Items.Any(i => i.CourseId == courseId))
                return Result<CartDto>.Failure("Course already in cart");

            // if not, create a cartItem and add it onto the cart
            var item = new CartItem
            {
                CourseId = course.CourseId,
                CourseTitle = course.Title,
                Price = course.Price,
                ImageUrl = course.ImageUrl ?? string.Empty,
                Subject = course.Subject ?? string.Empty,
                GradeLevel = course.GradeLevel.ToString(),
                Quantity = 1
            };

            // add the item 
            cart.Items.Add(item);


            // Persist to DB
            var result = await context.SaveChangesAsync() > 0;
            if (result)
                return Result<CartDto>.Success(cart.ToCartDto());

            return Result<CartDto>.Failure("Failed to add item to cart");
        }
        catch (Exception ex)
        {
            logger.LogError($"Error adding item to cart with cart id: {cartId} and course id:  {courseId}", ex);
            return Result<CartDto>.Failure("An error occurred while adding item to cart");
        }

    }

    public async Task<Result> ClearCartAsync(int id)
    {
        try
        {
            // Get the cart 
            var cart = await cartRepository.GetCartByIdAsync(id);
            if (cart is null) return Result.Failure("Cart not found");

            // if found clear the items
            context.CartItems.RemoveRange(cart.Items);

            if (await cartRepository.SaveChangesAsync())
                return Result.Success();

            return Result.Failure("Failed to clear cart");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error clearing cart {id}", id);
            return Result.Failure("An error occurred while clearing the cart");
        }
    }

    public async Task<Result<Cart>> RemoveItemAsync(int id, int courseId)
    {
        try
        {
            // Get the cart 
            var cart = await cartRepository.GetCartByIdAsync(id);
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
            logger.LogError($"Error removing an item from cart id: {id} and course id: {courseId}", ex);
            return Result<Cart>.Failure("An error occurred while removing item from cart");
        }
    }

    public async Task<Result<CartDto>> MergeCartsAsync(string guestCartId, int userId)
    {
        try
        {
            var guestCart = await context.Carts
                    .Include(c => c.Items)
                    .FirstOrDefaultAsync(c => c.CartId == guestCartId);

            var userCart = await context.Carts
                    .Include(c => c.Items)
                    .FirstOrDefaultAsync(c => c.UserId == userId);

            // No guest cart, nothing to merge
            if (guestCart is null)
            {
                return userCart is not null
                    ? Result<CartDto>.Success(userCart.ToCartDto())
                    : Result<CartDto>.Failure("No carts to merge");

            }

            if (userCart is null)
            {
                // Just attach guest cart to user 
                guestCart.UserId = userId;
                await context.SaveChangesAsync();
                return Result<CartDto>.Success(guestCart.ToCartDto());
            }

            // Merge logic 
            foreach (var guestItem in guestCart.Items)
            {
                if (!userCart.Items.Any(i => i.CourseId == guestItem.CourseId))
                {
                    userCart.Items.Add(guestItem);
                }
            }

            // Delete old guest cart to clean up
            context.Carts.Remove(guestCart);

            return Result<CartDto>.Success(userCart.ToCartDto());

        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error merging carts for user {userId}", userId);
            return Result<CartDto>.Failure("Failed to merge carts");
        }
    }
}
