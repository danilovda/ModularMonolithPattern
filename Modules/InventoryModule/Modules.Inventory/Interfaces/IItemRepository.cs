using Modules.Inventory.Models;

namespace Modules.Inventory.Interfaces;

internal interface IItemRepository
{
    Task<ItemDto?> Get(Guid id);
    Task<List<ItemDto>> GetAll();
    Task<bool> UpdateQuantity(UpdateQuantityDto updateQuantityDto);
}