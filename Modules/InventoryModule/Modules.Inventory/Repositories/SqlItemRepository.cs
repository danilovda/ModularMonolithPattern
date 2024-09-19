using Microsoft.EntityFrameworkCore;
using Modules.Inventory.Interfaces;
using Modules.Inventory.Models;

namespace Modules.Inventory.Repositories;

internal class SqlItemRepository(ProductDbContext dbContext) : IItemRepository
{
    private readonly ProductDbContext _dbContext = dbContext;    

    public async Task<ItemDto?> Get(Guid id)
    {
        var item = await _dbContext.Items.FirstOrDefaultAsync(x => x.Id == id);

        return item?.ToDto();
    }

    public async Task<List<ItemDto>> GetAll()
    {
        return await _dbContext.Items.Select(x => x.ToDto()).ToListAsync();
    }

    public async Task<bool> UpdateQuantity(UpdateQuantityDto updateQuantityDto)
    {
        var item = await _dbContext.Items.FirstOrDefaultAsync(i => i.Id == updateQuantityDto.ItemId);

        if (item is null) return false;

        item.Quantity += updateQuantityDto.Amount;

        await _dbContext.SaveChangesAsync();

        return true;
    }
}
