﻿using Modules.Inventory.Entities;
using Modules.Inventory.Interfaces;
using Modules.Inventory.Models;

namespace Modules.Inventory.Repositories;

internal class InMemoryItemRepository : IItemRepository
{
    private static readonly List<Item> _items =
    [
        new() { Id = Guid.Parse("111f9883-4b53-41eb-bcc3-8f4e6f29edf6"), Name = "Monitor", Quantity = 10, Price = 119.99 },
        new() { Id = Guid.Parse("bdabc506-3cac-47bf-b30b-a175e53cedfe"), Name = "Laptop", Quantity = 5, Price = 499.99 },
        new() { Id = Guid.Parse("e9386ab6-6a40-4fdf-876b-8efa7a3d30f0"), Name = "Keyboard", Quantity = 12, Price = 11.99 },
        new() { Id = Guid.Parse("14e92630-424a-4fd3-8657-4e56da9baf6b"), Name = "Mouse", Quantity = 15, Price = 7.99 }
    ];

    public async Task<ItemDto?> Get(Guid id)
    {
        var item = _items.FirstOrDefault(x => x.Id == id);

        await Task.CompletedTask;
        return item?.ToDto();
    }

    public async Task<List<ItemDto>> GetAll()
    {
        await Task.CompletedTask;
        return _items.Select(x => x.ToDto()).ToList();
    }

    public async Task<bool> UpdateQuantity(UpdateQuantityDto updateQuantityDto)
    {
        var item = _items.FirstOrDefault(i => i.Id == updateQuantityDto.ItemId);

        if (item is null) return false;

        item.Quantity += updateQuantityDto.Amount;

        await Task.CompletedTask;
        return true;
    }
}
