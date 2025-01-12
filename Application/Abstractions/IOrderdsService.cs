using Application.Models.Orders;

namespace Application.Abstractions;

public interface IOrdersService
{
    Task<OrderDto> Create(CreateOrderDto order);

    Task<OrderDto> GetById(Guid orderId);

    Task<List<OrderDto>> GetByUser(Guid customerId);

    Task<List<OrderDto>> GetAll();

    Task Reject(Guid orderId);
}