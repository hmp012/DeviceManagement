using DeviceManagament.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DeviceManagament.Database;

public class DeviceManagerDbContext(DbContextOptions<DeviceManagerDbContext> options)
    : DbContext(options)
{
    public static readonly string DefaultSchema = "devices";
    public DbSet<Device> Devices { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(DefaultSchema);
        
        modelBuilder.Entity<Device>(entity =>
        {
            entity.HasIndex(d => d.SerialNumber)
                .IsUnique();
            
            // Make init properties non-modifiable after insert
            entity.Property(d => d.SerialNumber)
                .ValueGeneratedNever()
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            
            entity.Property(d => d.ModelId)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            
            entity.Property(d => d.ModelName)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
            
            entity.Property(d => d.Manufacturer)
                .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Throw);
        });
    }
}