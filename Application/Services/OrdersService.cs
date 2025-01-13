using Application.Abstractions;
using Application.Mappers;
using Application.Models.Orders;
using Domain;
using Domain.Entities;
using Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

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
        if (order.Cart is null)
            throw new ArgumentNullException();

        var cart = await _cartsService.Create(order.Cart);

        var entity = new OrderEntity
        {
            OrderNumber = order.OrderNumber,
            Name = order.Name,
            CustomerId = order.CustomerId,
            CartId = cart.Id
        };

        await _dbContext.Orders.AddAsync(entity);
        await _dbContext.SaveChangesAsync();

        return entity.ToDto();
    }

    public async Task<OrderDto> GetById(long orderId)
    {
        var entity = await _dbContext.Orders
            .Include(x => x.Cart)
            .ThenInclude(x => x.CartItems)
            .FirstOrDefaultAsync(x => x.Id == orderId);

        return entity is null
            ? throw new EntityNotFoundException($"Order entity with id {orderId} not found")
            : entity.ToDto();
    }

    public async Task<List<OrderDto>> GetByUser(long customerId)
    {
        var entities = await _dbContext.Orders
            .Include(x => x.Cart)
            .ThenInclude(x => x.CartItems)
            .Where(x => x.CustomerId == customerId)
            .ToListAsync();

        return entities.Select(x => x.ToDto()).ToList();
    }

    public async Task<List<OrderDto>> GetAll()
    {
        var entities = await _dbContext.Orders
            .Include(x => x.Cart)
            .ThenInclude(x => x.CartItems)
            .ToListAsync();

        return entities.Select(x => x.ToDto()).ToList();
    }


    public Task Reject(long orderId)
    {
        throw new NotImplementedException();
    }
}