using Devices.Application.Abstractions;
using Devices.Application.Devices.Common;
using Devices.Domain.Common;
using Devices.Domain.Errors;
using Microsoft.Extensions.Logging;

namespace Devices.Application.Devices.Update;

public sealed class UpdateDeviceCommandHandler(
    IDeviceRepository repository,
    ILogger<UpdateDeviceCommandHandler> logger)
{
    public async Task<ResultT<DeviceResponse>> HandleAsync(
        UpdateDeviceCommand command,
        CancellationToken cancellationToken)
    {
        var device = await repository.GetByIdAsync(
            command.Id,
            cancellationToken);

        if (device is null)
        {
            logger.LogWarning(
                "[UpdateDeviceCommandHandler] - Device {DeviceId} was not found for update",
                command.Id);

            return ResultT<DeviceResponse>.Failure(DeviceErrors.NotFound);
        }

        var updateResult = device.Update(
            command.Name,
            command.Brand,
            command.State);

        if (!updateResult.IsSuccess)
        {
            logger.LogWarning(
                "[UpdateDeviceCommandHandler] - Update for device {DeviceId} was rejected. ErrorCode: {ErrorCode}",
                command.Id,
                updateResult.Error!.Code);

            return ResultT<DeviceResponse>.Failure(updateResult.Error!);
        }

        await repository.UpdateAsync(device, cancellationToken);

        logger.LogInformation(
            "[UpdateDeviceCommandHandler] - Device {DeviceId} was updated",
            device.Id);

        return ResultT<DeviceResponse>.Success(
            DeviceResponse.FromEntity(device));
    }
}
