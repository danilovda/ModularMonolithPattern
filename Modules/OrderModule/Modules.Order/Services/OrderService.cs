﻿using Modules.Inventory.Models;
using Modules.Orders.Interfaces;
using Modules.Orders.Models;

namespace Modules.Orders.Services;

internal class OrderService(
    IOrderRepository orderRepository,
    IInventoryRabbitMqClient inventoryRabbitMqClient,
    IInventoryRestClient inventoryRestClient) : IOrderService
{
    public async Task AddAsync(OrderDto orderDto)
    {
        foreach (var itemModel in orderDto.Items)
        {
            var item = await inventoryRestClient.GetItem(itemModel.ItemId) ??
                throw new InvalidOperationException($"The requested item '{itemModel.ItemId} was not found.'");

            if (item.Quantity < itemModel.Quantity)
                throw new InvalidOperationException($"There isn't enough stock for item '{itemModel.ItemId}'.");

            itemModel.PricePerUnit = item.Price;
            itemModel.Total = itemModel.Quantity * item.Price;

            var updateQuantityDto = new UpdateQuantityDto
            {
                ItemId = itemModel.ItemId,
                Amount = itemModel.Quantity * -1
            };

            inventoryRabbitMqClient.UpdateQuantity(updateQuantityDto);
        }

        orderDto.Total = orderDto.Items.Sum(i => i.Total);

        await orderRepository.Add(orderDto);        
    }

    public async Task<List<OrderDto>> GetAllAsync()
    {
        var orders = await orderRepository.GetAll();

        foreach (var orderItem in orders.SelectMany(i => i.Items))
        {
            var item = await inventoryRestClient.GetItem(orderItem.ItemId) ??
                throw new InvalidOperationException($"The requested item '{orderItem.ItemId} was not found.'");
            orderItem.ItemName = item.Name;
        }

        return orders;
    }
}
