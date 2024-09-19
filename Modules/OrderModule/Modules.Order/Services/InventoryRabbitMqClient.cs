using Modules.Inventory.Models;
using Modules.Orders.Interfaces;
using System.Text.Json;
using System.Text;
using Common.RabbitMq;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Modules.Orders.Services;

internal class InventoryRabbitMqClient(IOptions<RabbitMqConfiguration> rabbitMqConfiguration, IRabbitMqConnectionManager rabbitMqConnectionManager) : IInventoryRabbitMqClient
{
    public void UpdateQuantity(UpdateQuantityDto updateQuantityDto)
    {
        var serializedMessage = JsonSerializer.Serialize(updateQuantityDto);
        var body = Encoding.UTF8.GetBytes(serializedMessage);

        var queueName = rabbitMqConfiguration.Value.QueueName;

        rabbitMqConnectionManager.Channel.BasicPublish(
            exchange: string.Empty,                             
            routingKey: queueName,                             
            basicProperties: null,
            body: body);
    }
}
