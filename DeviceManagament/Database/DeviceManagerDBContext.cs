using DeviceManagament.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

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
        modelBuilder.Entity<Device>()
            .HasIndex(d => d.SerialNumber)
            .IsUnique();
    }
}
