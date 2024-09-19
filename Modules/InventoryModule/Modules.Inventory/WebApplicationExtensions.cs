using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Modules.Inventory.Interfaces;

namespace Modules.Inventory;

public static class WebApplicationExtensions
{
    public static void UseOrderConsumer(this WebApplication app)
    {
        var orderConsumer = app.Services.GetRequiredService<IRabbitMqConsumer>();
        orderConsumer.Consume();
    }
}
