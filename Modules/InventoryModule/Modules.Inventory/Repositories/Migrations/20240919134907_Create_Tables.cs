using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Modules.Inventory.Repositories.Migrations
{
    /// <inheritdoc />
    public partial class Create_Tables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "products");

            migrationBuilder.CreateTable(
                name: "Items",
                schema: "products",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "products",
                table: "Items",
                columns: new[] { "Id", "Name", "Price", "Quantity" },
                values: new object[,]
                {
                    { new Guid("111f9883-4b53-41eb-bcc3-8f4e6f29edf6"), "Monitor", 119.98999999999999, 10 },
                    { new Guid("14e92630-424a-4fd3-8657-4e56da9baf6b"), "Mouse", 7.9900000000000002, 15 },
                    { new Guid("bdabc506-3cac-47bf-b30b-a175e53cedfe"), "Laptop", 499.99000000000001, 5 },
                    { new Guid("e9386ab6-6a40-4fdf-876b-8efa7a3d30f0"), "Keyboard", 11.99, 12 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Items",
                schema: "products");
        }
    }
}
