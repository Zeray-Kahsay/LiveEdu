using API.DTOs.Orders;
using API.Helpers;

namespace API.Interfaces.Orders;

public interface IOrderService
{
    Task<Result<OrderDto>> CreateOrderAsync(CreateOrderDto dto);
    Task<Result<IEnumerable<OrderDto>>> GetUserOrderAsync(int userId);
    Task<Result<OrderDto>> GetOrderByIdAsync(int id);

}
