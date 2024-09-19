using Common.RabbitMq;
using Modules.Inventory;
using Modules.Orders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient();

builder.Services.AddRabbitMqConnectionManager(builder.Configuration);
builder.Services.AddInventoryModule(builder.Configuration);
builder.Services.AddOrderModule(builder.Configuration);

var app = builder.Build();

var appLifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();

app.UseRabbitMqConnectionManager();
app.UseOrderConsumer();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
