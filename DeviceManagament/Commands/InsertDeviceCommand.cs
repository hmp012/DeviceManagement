using DeviceManagament.Domain.DTOs;
using MediatR;

namespace DeviceManagament.Commands;

public record InsertDeviceCommand(DeviceDto Device) : IRequest<DeviceDto>;

public class InsertDeviceCommandHandler : IRequestHandler<InsertDeviceCommand, DeviceDto>
{
    // Inject your repository/DbContext here
    
    public async Task<DeviceDto> Handle(InsertDeviceCommand request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
