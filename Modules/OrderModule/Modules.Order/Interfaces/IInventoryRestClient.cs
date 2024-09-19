using Modules.Inventory.Models;

namespace Modules.Orders.Interfaces;

internal interface IInventoryRestClient
{
    Task<ItemDto?> GetItem(Guid id);
}
