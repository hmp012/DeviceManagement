using DeviceManagament.Domain.Models;

namespace DeviceManagament.Domain.DTOs;

public class DeviceDto
{
    public required string SerialNumber { get; set; }
    public required string ModelId { get; init; }
    public required string ModelName { get; init; }
    public required string Manufacturer { get; init; }
    public required string PrimaryUser { get; set; }
    public required string OperatingSystem { get; set; }
    public required string DeviceType { get; set; }
    public required string DeviceStatus { get; set; }
    
    public Device ToDevice()
    {
        return new Device
        {
            SerialNumber = new Guid(SerialNumber),
            ModelId = ModelId,
            ModelName = ModelName,
            Manufacturer = Manufacturer,
            PrimaryUser = PrimaryUser,
            OperatingSystem = OperatingSystem,
            DeviceType = Enum.Parse<DeviceType>(DeviceType),
            DeviceStatus = Enum.Parse<DeviceStatus>(DeviceStatus)
        };
    }
}