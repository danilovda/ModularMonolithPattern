using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Inventory.Interfaces;
using Modules.Inventory.Repositories;
using Modules.Inventory.Services;

namespace Modules.Inventory;

public static class ServiceCollectionExtensions
{
    public static void AddInventoryModule(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {        
        services.AddDbContext<ProductDbContext>(options => options
            .UseNpgsql(
                configuration.GetConnectionString("Db"),
                npgsqlOptions => npgsqlOptions.MigrationsHistoryTable(Schema.Products)));
        //services.AddTransient<IItemRepository, InMemoryItemRepository>();

        services.AddTransient<IItemRepository, SqlItemRepository>();
        services.AddTransient<IItemService, ItemService>();
        services.AddTransient<IRabbitMqConsumer, RabbitMqConsumer>();
    }
}
