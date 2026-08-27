using Devices.Application.Abstractions;
using Devices.Application.Devices.Common;
using Devices.Domain.Common;
using Devices.Domain.Entities;

namespace Devices.Application.Devices.Create;

public sealed class CreateDeviceCommandHandler(IDeviceRepository repository)
{
    public async Task<ResultT<DeviceResponse>> HandleAsync(
        CreateDeviceCommand command,
        CancellationToken cancellationToken)
    {
        var creationResult = Device.Create(
            command.Name,
            command.Brand,
            command.State);

        if (!creationResult.IsSuccess)
            return ResultT<DeviceResponse>.Failure(creationResult.Error!);

        var device = creationResult.Value!;
        await repository.AddAsync(device, cancellationToken);

        return ResultT<DeviceResponse>.Success(
            DeviceResponse.FromEntity(device));
    }
}
