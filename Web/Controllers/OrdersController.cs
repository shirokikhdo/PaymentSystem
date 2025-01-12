using System.Text.Json;
using Application.Abstractions;
using Application.Models.Orders;
using Microsoft.AspNetCore.Mvc;

namespace Web.Controllers;

[Route("api/orders")]
public class OrdersController : ApiBaseController
{
    private readonly IOrdersService _ordersService;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(
        IOrdersService ordersService, 
        ILogger<OrdersController> logger)
    {
        _ordersService = ordersService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<OrderDto>> Create(CreateOrderDto request)
    {
        _logger.LogInformation("Method api/order Create started." +
                               $"Request: {JsonSerializer.Serialize(request)}");

        var result = await _ordersService.Create(request);

        _logger.LogInformation("Method api/order Create finished." +
                               $"Request: {JsonSerializer.Serialize(request)}" + 
                               $"Response: {JsonSerializer.Serialize(result)}");

        return Ok(result);
    }
}