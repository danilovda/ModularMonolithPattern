using Microsoft.EntityFrameworkCore;
using Modules.Orders.Interfaces;
using Modules.Orders.Models;

namespace Modules.Orders.Repositories;

internal class SqlOrderRepository(OrderDbContext dbContext) : IOrderRepository
{    
    private readonly OrderDbContext _dbContext = dbContext;

    public async Task Add(OrderDto orderDto)
    {
        var order = new Entities.Order
        {
            Items = orderDto.Items.Select(i => new Entities.OrderItem
            {
                ItemId = i.ItemId,
                Id = i.Id,
                PricePerUnit = i.PricePerUnit,
                Quantity = i.Quantity,
                Total = i.Total,
            }).ToList()
        };

        await _dbContext.Orders.AddAsync(order);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<OrderDto>> GetAll()
    {
        return await _dbContext.Orders
            .Include(o => o.Items)
            .Select(o => o.ToDto())
            .ToListAsync();
    }
}
