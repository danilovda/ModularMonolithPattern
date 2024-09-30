using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Orders.Interfaces;
using Modules.Orders.Repositories;
using Modules.Orders.Repositories.Settings;
using Modules.Orders.Services;

namespace Modules.Orders;

public static class ServiceCollectionExtensions
{
    public static void AddOrderModule(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        var databaseSettings = configuration
            .GetSection($"{nameof(DatabaseSettings)}")
            .Get<DatabaseSettings>() ?? throw new Exception($"{nameof(DatabaseSettings)} is null");

        services.AddDbContext<OrderDbContext>(options => options
            .UseSqlServer(
                databaseSettings.ConnectionString,
                sqlOptions => sqlOptions.MigrationsHistoryTable(Schema.Orders)));
        
        //services.AddTransient<IOrderRepository, InMemoryOrderRepository>();

        services.AddTransient<IOrderRepository, SqlOrderRepository>();
        services.AddTransient<IOrderService, OrderService>();
        services.AddTransient<IInventoryRestClient, InventoryRestClient>();
        services.AddTransient<IInventoryRabbitMqClient, InventoryRabbitMqClient>();
    }
}
