using DeviceManagament.Domain.DTOs;
using MediatR;

namespace DeviceManagament.Commands;

public record UpdateDeviceCommand(Guid SerialNumber, DeviceDto Device) : IRequest<DeviceDto>;

public class UpdateDeviceCommandHandler : IRequestHandler<UpdateDeviceCommand, DeviceDto>
{
    // Inject your repository/DbContext here
    
    public async Task<DeviceDto> Handle(UpdateDeviceCommand request, CancellationToken cancellationToken)
    {
        // Update device logic
        throw new NotImplementedException();
    }
}
