namespace DeviceManagament.Exceptions;

public class InvalidDeviceDataException : Exception
{
    public InvalidDeviceDataException(string fieldName, string value, string reason)
        : base($"Invalid value '{value}' for field '{fieldName}': {reason}")
    {
    }
    
    public InvalidDeviceDataException(string message) : base(message)
    {
    }
}

