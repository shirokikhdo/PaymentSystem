using Application.Models.Carts;
using Application.Models.Orders;
using Domain.Entities;

namespace Application.Mappers;

public static class OrdersMapper
{
    public static OrderDto ToDto(this OrderEntity entity, CartEntity? cartEntity = null) =>
        new OrderDto
        {
            Id = entity.Id,
            CustomerId = entity.CustomerId!.Value,
            Cart = cartEntity is null
                ? entity.Cart?.ToDto()
                : cartEntity.ToDto(),
            Name = entity.Name,
            OrderNumber = entity.OrderNumber,
        };

    public static OrderEntity ToEntity(this CreateOrderDto orderDto, CartDto? cartDto = null) =>
        new OrderEntity
        {
            CustomerId = orderDto.CustomerId,
            Cart = cartDto?.ToEntity(),
            Name = orderDto.Name,
            OrderNumber = orderDto.OrderNumber,
        };
}