using Application.Models.Carts;
using Domain.Entities;

namespace Application.Mappers;

public static class CartsMapper
{
    public static CartDto ToDto(this CartEntity entity) =>
        new CartDto
        {
            Id = entity.Id,
            CartItems = entity.CartItems!
                .Select(item => new CartItemDto
                {
                    Id = item.Id,
                    Name = item.Name,
                    Price = item.Price,
                    Quantity = item.Quantity
                })
                .ToList()
        };

    public static CartEntity ToEntity(this CartDto cartDto) =>
        new CartEntity
        {
            CartItems = cartDto.CartItems
                .Select(x=>x.ToEntity())
                .ToList()
        };

    public static CartItemEntity ToEntity(this CartItemDto cartItemDto) =>
        new CartItemEntity
        {
            Name = cartItemDto.Name,
            Price = cartItemDto.Price,
            Quantity = cartItemDto.Quantity
        };
}