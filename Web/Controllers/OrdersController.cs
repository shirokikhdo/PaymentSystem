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

    [HttpGet]
    public async Task<ActionResult<List<OrderDto>>> GetAll()
    {
        _logger.LogInformation("Method api/orders GetAll started.");

        var result = await _ordersService.GetAll();

        _logger.LogInformation($"Method api/orders GetAll finished. Result count {result.Count}");

        return Ok(result);
    }

    [HttpGet("{orderId:long}")]
    public async Task<ActionResult<List<OrderDto>>> GetById(long orderId)
    {
        _logger.LogInformation($"Method api/orders/{orderId} GetById started.");

        var result = await _ordersService.GetById(orderId);

        _logger.LogInformation($"Method api/orders/{orderId} GetById finished." +
                                        $"Response: {JsonSerializer.Serialize(result)}");

        return Ok(result);
    }

    [HttpGet("customers/{customerId:long}")]
    public async Task<ActionResult<List<OrderDto>>> GetByUser(long customerId)
    {
        _logger.LogInformation($"Method api/orders/customers/{customerId} GetByUser started.");

        var result = await _ordersService.GetByUser(customerId);

        _logger.LogInformation($"Method api/orders/customers/{customerId} GetByUser finished. Result count {result.Count}");

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<OrderDto>> Create(CreateOrderDto request)
    {
        _logger.LogInformation("Method api/orders Create started." +
                               $"Request: {JsonSerializer.Serialize(request)}");

        var result = await _ordersService.Create(request);

        _logger.LogInformation("Method api/orders Create finished." +
                               $"Request: {JsonSerializer.Serialize(request)}" + 
                               $"Response: {JsonSerializer.Serialize(result)}");

        return Ok(result);
    }

    [HttpPost("{orderId:long}/reject")]
    public async Task<IActionResult> Reject(long orderId)
    {
        _logger.LogInformation($"Method api/orders/reject/{orderId} Reject started.");

        await _ordersService.Reject(orderId);

        _logger.LogInformation($"Method api/orders/reject/{orderId} Reject finished.");

        return Ok();
    }
}