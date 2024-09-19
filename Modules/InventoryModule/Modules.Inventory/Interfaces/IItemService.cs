using Modules.Inventory.Models;

namespace Modules.Inventory.Interfaces;

public interface IItemService
{
    Task<ItemDto?> Get(Guid id);
    Task<List<ItemDto>> GetAll();
    Task<bool> UpdateQuantity(UpdateQuantityDto updateQuantityDto);
}