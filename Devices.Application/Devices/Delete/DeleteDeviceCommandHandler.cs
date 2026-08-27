using Devices.Application.Abstractions;
using Devices.Domain.Common;
using Devices.Domain.Errors;

namespace Devices.Application.Devices.Delete;

public sealed class DeleteDeviceCommandHandler(IDeviceRepository repository)
{
    public async Task<Result> HandleAsync(
        DeleteDeviceCommand command,
        CancellationToken cancellationToken)
    {
        var device = await repository.GetByIdAsync(
            command.Id,
            cancellationToken);

        if (device is null)
            return Result.Failure(DeviceErrors.NotFound);

        var deletionResult = device.CanBeDeleted();
        
        if (!deletionResult)
            return Result.Failure(DeviceErrors.CannotDeleteInUse);

        await repository.RemoveAsync(device, cancellationToken);

        return Result.Success();
    }
}
