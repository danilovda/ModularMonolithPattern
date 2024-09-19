using Modules.Inventory.Interfaces;
using Modules.Inventory.Models;

namespace Modules.Inventory.Services;

internal class ItemService(IItemRepository itemRepository) : IItemService
{
    public async Task<ItemDto?> Get(Guid id)
    {
        return await itemRepository.Get(id);
    }

    public async Task<List<ItemDto>> GetAll()
    {
        return await itemRepository.GetAll();
    }

    public async Task<bool> UpdateQuantity(UpdateQuantityDto updateQuantityDto)
    {
        return await itemRepository.UpdateQuantity(updateQuantityDto);
    }
}