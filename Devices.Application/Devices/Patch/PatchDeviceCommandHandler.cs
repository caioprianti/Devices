using Devices.Application.Abstractions;
using Devices.Application.Devices.Common;
using Devices.Domain.Common;
using Devices.Domain.Errors;
using Microsoft.Extensions.Logging;

namespace Devices.Application.Devices.Patch;

public sealed class PatchDeviceCommandHandler(
    IDeviceRepository repository,
    ILogger<PatchDeviceCommandHandler> logger)
{
    public async Task<ResultT<DeviceResponse>> HandleAsync(
        PatchDeviceCommand command,
        CancellationToken cancellationToken)
    {
        var device = await repository.GetByIdAsync(
            command.Id,
            cancellationToken);

        if (device is null)
        {
            logger.LogWarning(
                "[PatchDeviceCommandHandler] - Device {DeviceId} was not found for partial update",
                command.Id);

            return ResultT<DeviceResponse>.Failure(DeviceErrors.NotFound);
        }

        var patchResult = device.Update(
            command.Name,
            command.Brand,
            command.State);

        if (!patchResult.IsSuccess)
        {
            logger.LogWarning(
                "[PatchDeviceCommandHandler] - Partial update for device {DeviceId} was rejected. ErrorCode: {ErrorCode}",
                command.Id,
                patchResult.Error!.Code);

            return ResultT<DeviceResponse>.Failure(patchResult.Error!);
        }

        await repository.UpdateAsync(device, cancellationToken);

        logger.LogInformation(
            "[PatchDeviceCommandHandler] - Device {DeviceId} was partially updated",
            device.Id);

        return ResultT<DeviceResponse>.Success(
            DeviceResponse.FromEntity(device));
    }
}
