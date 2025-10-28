// using System;
// using API.DTOs.Orders;
// using API.Entities.Carts;
// using API.Helpers;
// using API.Interfaces.Orders;

// namespace API.Repositories.Orders;

// public class OrderService(IOrderRepository orderRepository, ILogger<OrderService> logger) : IOrderService
// {
//     public async Task<Result<OrderDto>> CreateOrderAsync(CreateOrderDto dto)
//     {
//         if (dto.Items is null || dto.Items.Count == 0)
//             return Result<OrderDto>.Failure("Cart is empty");

//         var order = new Order
//         {
//             UserId = dto.UserId,
//             CreatedAt = DateTime.UtcNow,
//             IsPaid = false,
//             Items = dto.Items.Select(i => new OrderItem
//             {
//                 CourseId = i.CourseId,
//                 CourseTitle = i.Title,
//                 Quantity = i.Quantity,
//                 Price = i.Price,
//             }).ToList(),
//             Total = dto.Items.Sum(i => i.Price * i.Quantity),
//         };

//         await orderRepository.AddOrderAsync(order);
//         var saved = await orderRepository.SaveChangesAsync();
//         if (!saved) return Result<OrderDto>.Failure("Failed to save order");

//         var dtoOut = new OrderDto
//         {
//             OrderId = order.OrderId,
//             UserId = order.UserId,
//             CreatedAt = order.CreatedAt,
//             IsPaid = order.IsPaid,
//             Total = order.Total,
//             Items = order.Items.Select(i => new OrderItemDto
//             {
//                 CourseId = i.CourseId,
//                 CourseTitle = i.CourseTitle,
//                 Quantity = i.Quantity,
//                 Price = i.Price
//             }).ToList()
//         };

//         // TODO: Enroll the student in the courses

//         return Result<OrderDto>.Success(dtoOut);
//     }

//     public async Task<Result<OrderDto>> GetOrderByIdAsync(int id)
//     {
//         var order = await orderRepository.GetOrderByIdAsync(id);
//         if (order is null)
//         {
//             logger.LogInformation("Order with ID {OrderId} not found.", id);

//             return Result<OrderDto>.Failure("Order not found");
//         }

//         var dtoOut = new OrderDto
//         {
//             OrderId = order.OrderId,
//             UserId = order.UserId,
//             CreatedAt = order.CreatedAt,
//             IsPaid = order.IsPaid,
//             Total = order.Total,
//             Items = order.Items.Select(i => new OrderItemDto
//             {
//                 CourseId = i.CourseId,
//                 CourseTitle = i.CourseTitle,
//                 Quantity = i.Quantity,
//                 Price = i.Price
//             }).ToList()
//         };

//         return Result<OrderDto>.Success(dtoOut);
//     }

//     public async Task<Result<IEnumerable<OrderDto>>> GetUserOrderAsync(int userId)
//     {
//         var orders = await orderRepository.GetOrdersByUserIdAsync(userId);
//         if (orders is null || !orders.Any())
//             return Result<IEnumerable<OrderDto>>.Failure("No orders found for user");

//         var dtoOut = orders.Select(order => new OrderDto
//         {
//             OrderId = order.OrderId,
//             UserId = order.UserId,
//             CreatedAt = order.CreatedAt,
//             IsPaid = order.IsPaid,
//             Total = order.Total,
//             Items = order.Items.Select(i => new OrderItemDto
//             {
//                 CourseId = i.CourseId,
//                 CourseTitle = i.CourseTitle,
//                 Quantity = i.Quantity,
//                 Price = i.Price
//             }).ToList()
//         });

//         return Result<IEnumerable<OrderDto>>.Success(dtoOut);

//     }
// }
