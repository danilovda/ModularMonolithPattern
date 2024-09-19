using Modules.Orders.Models;

namespace Modules.Orders.Interfaces;

internal interface IOrderRepository
{
    Task Add(OrderDto orderDto);
    Task<List<OrderDto>> GetAll();
}
