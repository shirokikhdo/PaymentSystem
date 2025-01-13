﻿using Application.Models.Orders;

namespace Application.Abstractions;

public interface IOrdersService
{
    Task<OrderDto> Create(CreateOrderDto order);

    Task<OrderDto> GetById(long orderId);

    Task<List<OrderDto>> GetByUser(long customerId);

    Task<List<OrderDto>> GetAll();

    Task Reject(long orderId);
}