using DeviceManagament.Domain.DTOs;
using DeviceManagament.Domain.Models;
using DeviceManagament.Repositories;
using MediatR;

namespace DeviceManagament.Commands;

public record UpdateDeviceCommand(Guid SerialNumber, DeviceDto DeviceDto) : IRequest<DeviceDto>;

public class UpdateDeviceCommandHandler(IDeviceRepository deviceRepository) : IRequestHandler<UpdateDeviceCommand, DeviceDto>
{
    public async Task<DeviceDto> Handle(UpdateDeviceCommand request, CancellationToken cancellationToken)
    {
        var deviceToUpdate = request.DeviceDto.ToDevice(); // Todo: check all fields can are valid so no unhandled error is thrown
        if (request.SerialNumber.ToString() != request.DeviceDto.SerialNumber)
        {
            throw new ArgumentException("Serial number in the route does not match the serial number in the body."); // TODO: Implement in ExceptionFilter
        }

        Device device = await deviceRepository.UpdateDevice(deviceToUpdate);
        return device.ToDeviceDto();
    }
}
