using API.DTOs.Orders;
using API.Helpers;
using API.Interfaces.Orders;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class OrdersController(IOrderService orderService, ILogger<OrdersController> logger) : BaseController
{

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        var result = await orderService.GetOrderByIdAsync(id);
        if (!result.IsSuccess)
        {
            logger.LogError("Failed to retrieve order {OrderId}: {Error}", id, result.Errors);
            return NotFound(new ApiErrorDto
            {
                Status = 404,
                Message = "Order not found",
                Errors = result.Errors
            });
        }

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto dto)
    {
        var result = await orderService.CreateOrderAsync(dto);
        if (!result.IsSuccess)
        {
            logger.LogError("Failed to create order: {Error}", result.Errors);
            return BadRequest(new ApiErrorDto
            {
                Status = 400,
                Message = "Order creation failed",
                Errors = result.Errors

            });
        }

        return CreatedAtAction(nameof(GetOrderById), new { id = result.Value!.OrderId }, result.Value);
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserOrders(int userId)
    {
        var result = await orderService.GetUserOrderAsync(userId);
        if (!result.IsSuccess)
        {
            logger.LogError("Failed to retrieve orders for user {UserId}: {Error}", userId, result.Errors);
            return NotFound(new ApiErrorDto
            {
                Status = 404,
                Message = "Orders not found",
                Errors = result.Errors
            });
        }

        return Ok(result.Value);
    }
}
