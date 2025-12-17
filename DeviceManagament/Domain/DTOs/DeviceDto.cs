using DeviceManagament.Domain.Models;
using DeviceManagament.Exceptions;

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
        // Parse SerialNumber GUID with error handling
        Guid serialNumberGuid;
        try
        {
            serialNumberGuid = new Guid(SerialNumber);
        }
        catch (Exception ex) when (ex is FormatException or ArgumentNullException or OverflowException)
        {
            throw new InvalidDeviceDataException("SerialNumber", SerialNumber, "Must be a valid GUID format");
        }
        
        // Parse DeviceType with error handling
        if (!Enum.TryParse<DeviceType>(DeviceType, true, out var deviceType))
        {
            var validValues = string.Join(", ", Enum.GetNames(typeof(DeviceType)));
            throw new InvalidDeviceDataException("DeviceType", DeviceType, $"Must be one of: {validValues}");
        }
        
        // Parse DeviceStatus with error handling
        if (!Enum.TryParse<DeviceStatus>(DeviceStatus, true, out var deviceStatus))
        {
            var validValues = string.Join(", ", Enum.GetNames(typeof(DeviceStatus)));
            throw new InvalidDeviceDataException("DeviceStatus", DeviceStatus, $"Must be one of: {validValues}");
        }
        
        return new Device
        {
            SerialNumber = serialNumberGuid,
            ModelId = ModelId,
            ModelName = ModelName,
            Manufacturer = Manufacturer,
            PrimaryUser = PrimaryUser,
            OperatingSystem = OperatingSystem,
            DeviceType = deviceType,
            DeviceStatus = deviceStatus
        };
    }
}