using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Modules.Inventory.Entities;
using System.Reflection;

namespace Modules.Inventory.Repositories;

internal class ProductDbContext : DbContext
{
    private readonly ILogger<ProductDbContext> _logger;

    public DbSet<Item> Items { get; set; }

    public ProductDbContext(
        DbContextOptions<ProductDbContext> options,
        ILogger<ProductDbContext> logger) : base(options)
    {
        _logger = logger;
        _logger.LogDebug(
            "-->Database.EnsureCreating ({ApplicationContext})",
            Assembly.GetExecutingAssembly().GetName().Name);
        
        Database.Migrate();

        //Add-Migration Create_Tables -Context ProductDbContext -o Repositories/Migrations
        //Update-Database -Context OrderDbContext

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema(Schema.Products);

        modelBuilder.Entity<Item>().HasData(new Item() { Id = Guid.Parse("111f9883-4b53-41eb-bcc3-8f4e6f29edf6"), Name = "Monitor", Quantity = 10, Price = 119.99 });
        modelBuilder.Entity<Item>().HasData(new Item() { Id = Guid.Parse("bdabc506-3cac-47bf-b30b-a175e53cedfe"), Name = "Laptop", Quantity = 5, Price = 499.99 });
        modelBuilder.Entity<Item>().HasData(new Item() { Id = Guid.Parse("e9386ab6-6a40-4fdf-876b-8efa7a3d30f0"), Name = "Keyboard", Quantity = 12, Price = 11.99 });
        modelBuilder.Entity<Item>().HasData(new Item() { Id = Guid.Parse("14e92630-424a-4fd3-8657-4e56da9baf6b"), Name = "Mouse", Quantity = 15, Price = 7.99 });
    }
}