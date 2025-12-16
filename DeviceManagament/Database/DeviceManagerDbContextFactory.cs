using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DeviceManagament.Database;

public class DeviceManagerDbContextFactory : IDesignTimeDbContextFactory<DeviceManagerDbContext>
{
    public DeviceManagerDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.local.json", optional: false)
            .Build();

        var connectionString = configuration.GetConnectionString("DeviceManagerDB")
                               ?? throw new InvalidOperationException("DeviceManagerDB connection string is required");

        var optionsBuilder = new DbContextOptionsBuilder<DeviceManagerDbContext>();
        optionsBuilder.UseNpgsql(connectionString);

        return new DeviceManagerDbContext(optionsBuilder.Options);
    }
}