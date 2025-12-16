using DeviceManagament.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace DeviceManagament.Repositories;

public interface IDeviceRepository
{
    public Task<Device> AddDevice(Device device);
    public Task<Device?> GetDevice(Device device);
    public Task<Device> UpdateDevice(Device device);
}

public class DeviceRepository(Database.DeviceManagerDbContext dbContext) : IDeviceRepository
{
    public async Task<Device> AddDevice(Device device)
    {
        EntityEntry<Device> addedEntity = await dbContext.Devices.AddAsync(device);
        await dbContext.SaveChangesAsync();
        return addedEntity.Entity;
    }

    public async Task<Device?> GetDevice(Device device)
    {
        return await dbContext.Devices
            .FirstOrDefaultAsync(d => d.SerialNumber == device.SerialNumber);
    }

    public async Task<Device> UpdateDevice(Device device)
    {
        EntityEntry<Device> updatedEntity = dbContext.Devices.Update(device);
        await dbContext.SaveChangesAsync();
        return updatedEntity.Entity;
    }
}