using Modules.Orders.Models;

namespace Modules.Orders.Interfaces;

public interface IOrderService
{
    Task AddAsync(OrderDto orderDto);
    Task<List<OrderDto>> GetAllAsync();
}
