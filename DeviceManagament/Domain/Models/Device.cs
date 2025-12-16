using System.ComponentModel.DataAnnotations;

namespace DeviceManagament.Domain.Models;

public class Device
{
    [Key]
    public Guid SerialNumber { get; init; }
    public required string ModelId { get; init; }
    public required string ModelName { get; init; }
    public required string Manufacturer { get; init; }
    [EmailAddress]
    public required string PrimaryUser { get; set; }
    public required string OperatingSystem { get; set; }
    public required DeviceType DeviceType { get; set; }
    public required DeviceStatus DeviceStatus { get; set; }
}

public enum DeviceType
{
    Laptop,
    Desktop
}

public enum DeviceStatus
{
    Active,
    Inactive,
    Retired
}