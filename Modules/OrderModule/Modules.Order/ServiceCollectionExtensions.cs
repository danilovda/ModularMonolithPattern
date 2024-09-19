using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Modules.Orders.Interfaces;
using Modules.Orders.Repositories;
using Modules.Orders.Services;

namespace Modules.Orders;

public static class ServiceCollectionExtensions
{
    public static void AddOrderModule(
        this IServiceCollection services,
        ConfigurationManager configuration)
    {
        services.AddDbContext<OrderDbContext>(options => options
            .UseNpgsql(
                configuration.GetConnectionString("Db"),
                npgsqlOptions => npgsqlOptions.MigrationsHistoryTable(Schema.Orders)));
        //services.AddTransient<IOrderRepository, InMemoryOrderRepository>();

        services.AddTransient<IOrderRepository, SqlOrderRepository>();
        services.AddTransient<IOrderService, OrderService>();
        services.AddTransient<IInventoryRestClient, InventoryRestClient>();
        services.AddTransient<IInventoryRabbitMqClient, InventoryRabbitMqClient>();
    }
}
