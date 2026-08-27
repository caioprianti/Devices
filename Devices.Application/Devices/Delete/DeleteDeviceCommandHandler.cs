using Devices.Application.Abstractions;
using Devices.Domain.Common;
using Devices.Domain.Errors;
using Microsoft.Extensions.Logging;

namespace Devices.Application.Devices.Delete;

public sealed class DeleteDeviceCommandHandler(
    IDeviceRepository repository,
    ILogger<DeleteDeviceCommandHandler> logger)
{
    public async Task<Result> HandleAsync(
        DeleteDeviceCommand command,
        CancellationToken cancellationToken)
    {
        var device = await repository.GetByIdAsync(
            command.Id,
            cancellationToken);

        if (device is null)
        {
            logger.LogWarning(
                "[DeleteDeviceCommandHandler] - Device {DeviceId} was not found for deletion",
                command.Id);

            return Result.Failure(DeviceErrors.NotFound);
        }

        var deletionResult = device.CanBeDeleted();
        
        if (!deletionResult)
        {
            logger.LogWarning(
                "[DeleteDeviceCommandHandler] - Deletion for device {DeviceId} was rejected. ErrorCode: {ErrorCode}",
                command.Id,
                DeviceErrors.CannotDeleteInUse.Code);

            return Result.Failure(DeviceErrors.CannotDeleteInUse);
        }

        await repository.RemoveAsync(device, cancellationToken);

        logger.LogInformation(
            "[DeleteDeviceCommandHandler] - Device {DeviceId} was deleted",
            device.Id);

        return Result.Success();
    }
}
