using API.Entities.Carts;
using API.Helpers;
using API.Repositories.Carts;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class CartsController(CartService cartService) : BaseController
{
    [HttpGet("{id}")]
    public async Task<ActionResult<Cart>> GetCart(string id)
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


    [HttpPost("{cartId}/add/{courseId}")]
    public async Task<ActionResult> AddItem(string cartId, int courseId)
    {
        var result = await cartService.AddItemAsync(cartId, courseId);
        if (!result.IsSuccess)
            return BadRequest(new ApiErrorDto
            {
                Status = 400,
                Message = "Unable to add iten",
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

        return Ok();
    }
}
