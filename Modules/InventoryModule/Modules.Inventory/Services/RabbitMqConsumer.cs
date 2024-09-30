using Common.RabbitMq;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Modules.Inventory.Interfaces;
using Modules.Inventory.Models;
using RabbitMQ.Client.Events;
using System.Text.Json;
using System.Text;
using RabbitMQ.Client;
using OpenTelemetry.Context.Propagation;
using System.Diagnostics;
using OpenTelemetry;
using Microsoft.Extensions.Logging;

namespace Modules.Inventory.Services;

public class RabbitMqConsumer(
    ILogger<RabbitMqConsumer> logger,
    IServiceScopeFactory scopeFactory,
    IOptions<RabbitMqConfiguration> rabbitMqConfiguration,
    IRabbitMqConnectionManager rabbitMqConnectionManager) : IRabbitMqConsumer
{
    private static readonly ActivitySource Activity = new(nameof(RabbitMqConsumer));
    private static readonly TraceContextPropagator Propagator = new();

    public void Consume()
    {
        var queueName = rabbitMqConfiguration.Value.QueueName;
        rabbitMqConnectionManager.Channel?.QueueDeclare(        
            queue: queueName,
            durable: false,
            exclusive: false,
            autoDelete: false,
            arguments: null);

        var consumer = new EventingBasicConsumer(rabbitMqConnectionManager.Channel);

        consumer.Received += async (model, ea) => await HandleEventAsync(ea);                

        rabbitMqConnectionManager.Channel.BasicConsume(
            queue: queueName,
            autoAck: true,
            consumer: consumer);
    }

    private async Task HandleEventAsync(BasicDeliverEventArgs ea)
    {
        var parentContext = Propagator.Extract(default, ea.BasicProperties, ExtractTraceContextFromBasicProperties);
        Baggage.Current = parentContext.Baggage;


        using (var activity = Activity.StartActivity("Process Message", ActivityKind.Consumer, parentContext.ActivityContext))
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);
            var updateQuantityDto = JsonSerializer.Deserialize<UpdateQuantityDto>(message);

            if (activity != null)
            {   
                RabbitMqHelper.AddMessagingTags(activity, rabbitMqConfiguration.Value.QueueName);
            }

            if (updateQuantityDto is null)
                return;

            await UpdateItemQuantity(updateQuantityDto);
        }        
    }

    private async Task UpdateItemQuantity(UpdateQuantityDto updateQuantityDto)
    {
        var scope = scopeFactory.CreateScope();
        var itemService = scope.ServiceProvider.GetRequiredService<IItemService>();

        await itemService.UpdateQuantity(updateQuantityDto);
    }

    private IEnumerable<string> ExtractTraceContextFromBasicProperties(IBasicProperties props, string key)
    {
        try
        {
            if (props.Headers != null)
            {
                if (props.Headers.TryGetValue(key, out var value))
                {
                    var bytes = value as byte[];
                    if (bytes != null)                    
                        return [Encoding.UTF8.GetString(bytes)];
                }
            }
            
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to extract trace context");
        }

        return Enumerable.Empty<string>();
    }
}