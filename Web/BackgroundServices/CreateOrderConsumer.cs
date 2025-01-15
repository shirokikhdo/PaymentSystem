using System.Text;
using System.Text.Json;
using Application.Abstractions;
using Application.Models.Orders;
using Domain.Options;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Web.BackgroundServices;

public class CreateOrderConsumer : BackgroundService
{
    private readonly RabbitMqOptions _rabbitMqOptions;
    private readonly IChannel _channel;
    private readonly IServiceProvider _serviceProvider;

    public CreateOrderConsumer(
        IOptions<RabbitMqOptions> options, 
        IServiceProvider serviceProvider)
    {
        _rabbitMqOptions = options.Value;
        _serviceProvider = serviceProvider;
        var factory = new ConnectionFactory
        {
            HostName = _rabbitMqOptions.HostName,
            Port = _rabbitMqOptions.Port,
            UserName = _rabbitMqOptions.UserName,
            Password = _rabbitMqOptions.Password,
            VirtualHost = _rabbitMqOptions.VirtualHost
        };

        var connection = factory.CreateConnectionAsync()
            .GetAwaiter()
            .GetResult();
        _channel = connection.CreateChannelAsync()
            .GetAwaiter()
            .GetResult();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (_, args) =>
        {
            var body = args.Body;
            var message = Encoding.UTF8.GetString(body.ToArray());
            try
            {
                var createOrderDto = JsonSerializer.Deserialize<CreateOrderDto>(message, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                })!;
                using var scope = _serviceProvider.CreateScope();
                var ordersService = scope.ServiceProvider.GetRequiredService<IOrdersService>();

                await ordersService.Create(createOrderDto);

                await _channel.BasicAckAsync(args.DeliveryTag, false, stoppingToken);
            }
            catch (Exception)
            {
                await _channel.BasicAckAsync(args.DeliveryTag, false, stoppingToken);
            }
        };

        await _channel.BasicConsumeAsync(_rabbitMqOptions.CreateOrderQueueName, 
            autoAck: false, 
            consumer, 
            cancellationToken: stoppingToken);
    }
}