using API.Entities.Carts;
using API.Extensions;
using API.Helpers;
using API.Repositories.Carts;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class CartsController(CartService cartService) : BaseController
{
    [HttpGet("{id}")]
    public async Task<ActionResult<Cart>> GetCart(int id)
    {
        var result = await cartService.GetCartAsync(id);
        if (!result.IsSuccess)
            return NotFound(new ApiErrorDto
            {
                Status = 404,
                Message = "Car not found",
                Errors = result.Errors
            });

        return Ok(result.Value);
    }


    [HttpPost("add/{courseId}")]
    public async Task<ActionResult> AddItem(int courseId, [FromQuery] string? cartId = null)
    {
        var userId = User.GetUserId();
        var result = await cartService.AddItemAsync(courseId, cartId, userId);
        if (!result.IsSuccess)
            return BadRequest(new ApiErrorDto
            {
                Status = 400,
                Message = "Unable to add item",
                Errors = result.Errors
            });

        return Ok(result.Value);
    }


    [HttpDelete("{cartId}/remove/{courseId}")]
    public async Task<ActionResult> RemoveItem(string cartId, int courseId)
    {
        var result = await cartService.RemoveItemAsync(cartId, courseId);
        if (!result.IsSuccess)
            return BadRequest(new ApiErrorDto { Status = 400, Message = "Unable to remove item", Errors = result.Errors });
        return Ok(result.Value);
    }


    [HttpDelete("{cartId}/clear")]
    public async Task<ActionResult> ClearCart(string cartId)
    {
        var result = await cartService.ClearCartAsync(cartId);
        if (!result.IsSuccess)
            return BadRequest(new ApiErrorDto
            {
                Status = 400,
                Message = "Failed to clear cart",
                Errors = result.Errors
            });

        return Ok(result.Value);
    }
}
