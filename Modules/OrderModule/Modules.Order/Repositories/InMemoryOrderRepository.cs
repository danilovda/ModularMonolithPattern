using Modules.Orders.Interfaces;
using Modules.Orders.Models;

namespace Modules.Orders.Repositories;

internal class InMemoryOrderRepository : IOrderRepository
{
    private static readonly List<Entities.Order> _orders = [];

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

        _orders.Add(order);

        await Task.CompletedTask;
    }

    public async Task<List<OrderDto>> GetAll()
    {
        await Task.CompletedTask;
        return _orders.Select(i => i.ToDto()).ToList();
    }
}
