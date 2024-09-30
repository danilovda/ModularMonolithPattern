using Common.RabbitMq;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Modules.Inventory.Models;
using Modules.Orders.Controllers;
using Modules.Orders.Interfaces;
using OpenTelemetry;
using OpenTelemetry.Context.Propagation;
using RabbitMQ.Client;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Modules.Orders.Services;

internal class InventoryRabbitMqClient(
    ILogger<InventoryRabbitMqClient> logger,
    IOptions<RabbitMqConfiguration> rabbitMqConfiguration,
    IRabbitMqConnectionManager rabbitMqConnectionManager) : IInventoryRabbitMqClient
{
    private static readonly ActivitySource Activity = new(nameof(OrderController));
    private static readonly TextMapPropagator Propagator = Propagators.DefaultTextMapPropagator;

    public void UpdateQuantity(UpdateQuantityDto updateQuantityDto)
    {
        using (var activity = Activity.StartActivity("RabbitMq Publish", ActivityKind.Producer))
        {
            var props = rabbitMqConnectionManager.Channel?.CreateBasicProperties();

            if (activity != null && props != null)
                AddActivityToHeader(activity, props);

            var serializedMessage = JsonSerializer.Serialize(updateQuantityDto);
            var body = Encoding.UTF8.GetBytes(serializedMessage);

            var queueName = rabbitMqConfiguration.Value.QueueName;

            rabbitMqConnectionManager.Channel.BasicPublish(
                exchange: string.Empty,
                routingKey: queueName,
                basicProperties: props,
                body: body);
        }
    }

    private void AddActivityToHeader(Activity activity, IBasicProperties props)
    {
        Propagator.Inject(new PropagationContext(activity.Context, Baggage.Current), props, InjectContextIntoHeader);
        RabbitMqHelper.AddMessagingTags(activity, rabbitMqConfiguration.Value.QueueName);
    }

    private void InjectContextIntoHeader(IBasicProperties props, string key, string value)
    {
        try
        {
            props.Headers ??= new Dictionary<string, object>();
            props.Headers[key] = value;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to inject trace context.");
        }
    }

}
