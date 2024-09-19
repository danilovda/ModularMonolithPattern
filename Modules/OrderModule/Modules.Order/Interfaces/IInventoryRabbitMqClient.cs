using Modules.Inventory.Models;

namespace Modules.Orders.Interfaces;

internal interface IInventoryRabbitMqClient
{
    void UpdateQuantity(UpdateQuantityDto updateQuantityDto);
}
