using DeviceManagament.Domain.DTOs;
using DeviceManagament.Domain.Models;
using DeviceManagament.Exceptions;
using DeviceManagament.Repositories;
using MediatR;

namespace DeviceManagament.Commands;

public record InsertDeviceCommand(
    DeviceDto DeviceDto) : IRequest<DeviceDto>
{
    public class InsertDeviceCommandHandler(
        IDeviceRepository deviceRepository,
        ILogger<InsertDeviceCommand> logger) : IRequestHandler<InsertDeviceCommand, DeviceDto>
    {
        public async Task<DeviceDto> Handle(InsertDeviceCommand request, CancellationToken cancellationToken)
        {
            Device deviceRequest = request.DeviceDto.ToDevice(); // TODO: check all fields can are valid so no unhandled error is thrown
            Device? deviceCheck = await deviceRepository.GetDevice(deviceRequest);
            if (deviceCheck != null)
            {
                logger.LogWarning("Device with Serial Number {SerialNumber} already exists.", deviceCheck.SerialNumber);
                throw new DeviceAlreadyExistsException(deviceCheck.SerialNumber.ToString());
            }


            Device device = await deviceRepository.AddDevice(deviceRequest);
            return device.ToDeviceDto();
        }
    }
}