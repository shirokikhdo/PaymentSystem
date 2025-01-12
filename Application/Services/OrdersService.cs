using Application.Abstractions;
using Application.Models.Orders;
using Domain;
using Domain.Entities;

namespace Application.Services;

public class OrdersService : IOrdersService
{
    private readonly PaymentSystemDbContext _dbContext;
    private readonly ICartsService _cartsService;

    public OrdersService(PaymentSystemDbContext dbContext, ICartsService cartService)
    {
        _dbContext = dbContext;
        _cartsService = cartService;
    }

    public async Task<OrderDto> Create(CreateOrderDto order)
    {
        var cart = await _cartsService.Create(order.Cart);

        var entity = new OrderEntity
        {
            OrderNumber = order.OrderNumber,
            Name = order.Name,
            CustomerId = order.CustomerId,
            CartId = cart.Id
        };

        var orderSaveResult = await _dbContext.Orders.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return new OrderDto
        {
            Id = orderSaveResult.Entity.Id,
            CustomerId = orderSaveResult.Entity.CustomerId!.Value,
            Cart = cart,
            Name = orderSaveResult.Entity.Name,
            OrderNumber = orderSaveResult.Entity.OrderNumber,
        };
    }

    public Task<OrderDto> GetById(Guid orderId)
    {
        throw new NotImplementedException();
    }

    public Task<List<OrderDto>> GetByUser(Guid customerId)
    {
        throw new NotImplementedException();
    }

    public Task<List<OrderDto>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task Reject(Guid orderId)
    {
        throw new NotImplementedException();
    }
}