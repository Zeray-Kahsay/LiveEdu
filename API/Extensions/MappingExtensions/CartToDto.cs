using API.DTOs.Carts;
using API.Entities.Carts;

namespace API.Extensions.MappingExtensions;

public static class CartToDto
{
    public static CartDto ToCartDto(this Cart cart)
    {
        return new CartDto
        {
            CartId = cart.CartId, // Cart's manually generated key, usefull in the client side 
            Id = cart.Id, // car's PK
            UserId = cart.UserId,
            PaymentIntentId = cart.PaymentIntentId,
            ClientSecret = cart.ClientSecretId,
            Items = cart.Items.Select(it => new CartItemDto
            {
                CourseId = it.CourseId,
                Title = it.CourseTitle,
                ImageUrl = it.ImageUrl,
                Price = it.Price,
                Quantity = it.Quantity,
                Subject = it.Subject,
                GradeLevel = it.GradeLevel,
                TeacherName = it.Course != null
                                ? $"{it.Course.Teacher?.FirstName} {it.Course.Teacher?.LastName}"
                                : "Teacher name unavialable",
                Description = it.Course != null
                                ? it.Course.Description
                                : "Course description unavailable",
            }).ToList()


        };
    }

}
