using System;

namespace API.DTOs.Carts;

public class CartDto
{
    public int Id { get; set; }
    public string CartId { get; set; } = string.Empty;
    public int? UserId { get; set; }
    public ICollection<CartItemDto> Items { get; set; } = [];
    public string? PaymentIntentId { get; set; }
    public string? ClientSecret { get; set; }
    public string PaymentStatus { get; set; } = string.Empty;
}
