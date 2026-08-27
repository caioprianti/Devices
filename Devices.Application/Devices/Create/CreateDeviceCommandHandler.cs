using Devices.Application.Abstractions;
using Devices.Application.Devices.Common;
using Devices.Domain.Common;
using Devices.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Devices.Application.Devices.Create;

public sealed class CreateDeviceCommandHandler(
    IDeviceRepository repository,
    ILogger<CreateDeviceCommandHandler> logger)
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
        {
            logger.LogWarning(
                "[CreateDeviceCommandHandler] - Device creation was rejected. ErrorCode: {ErrorCode}",
                creationResult.Error!.Code);

            return ResultT<DeviceResponse>.Failure(creationResult.Error!);
        }

        var device = creationResult.Value!;
        await repository.AddAsync(device, cancellationToken);

        logger.LogInformation(
            "[CreateDeviceCommandHandler] - Device {DeviceId} was created with brand {Brand} and state {State}",
            device.Id,
            device.Brand,
            device.State);

        return ResultT<DeviceResponse>.Success(
            DeviceResponse.FromEntity(device));
    }
}
