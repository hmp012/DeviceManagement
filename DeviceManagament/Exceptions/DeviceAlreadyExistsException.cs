namespace DeviceManagament.Exceptions;

public class DeviceAlreadyExistsException(string serialNumber)
    : Exception($"Device with Serial Number {serialNumber} already exists.");
