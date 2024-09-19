using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modules.Orders.Entities;

using System.Reflection;

namespace Modules.Orders.Repositories;

internal class OrderDbContext : DbContext
{
    private readonly ILogger<OrderDbContext> _logger;

    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    public OrderDbContext(
        DbContextOptions<OrderDbContext> options,
        ILogger<OrderDbContext> logger) : base(options)
    {
        _logger = logger;
        _logger.LogDebug(
            "-->Database.EnsureCreating ({ApplicationContext})",
            Assembly.GetExecutingAssembly().GetName().Name);
        
        Database.Migrate();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(Schema.Orders);
    }
}