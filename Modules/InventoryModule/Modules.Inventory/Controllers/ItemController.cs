using Microsoft.AspNetCore.Mvc;
using Modules.Inventory.Interfaces;

namespace Modules.Inventory.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ItemController(IItemService itemService) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var item = await itemService.Get(id);

        return item is null
            ? NotFound(id)
            : Ok(item);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await itemService.GetAll();

        return Ok(items);
    }
}