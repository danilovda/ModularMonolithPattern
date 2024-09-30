namespace Modules.Inventory.Repositories.Settings;

public class DatabaseSettings
{
    public string Host { get; set; } = null!;
    public string Catalog { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public string UserPassword { get; set; } = null!;
    public int Port { get; set; }
    public string ConnectionString => $"Server={Host},{Port};Initial Catalog={Catalog};User Id={UserId};Password={UserPassword};Encrypt=false";
}
