using Devices.Application.Abstractions;
using Devices.Application.Devices.Common;
using Devices.Domain.Common;
using Devices.Domain.Errors;

namespace Devices.Application.Devices.Update;

public sealed class UpdateDeviceCommandHandler(IDeviceRepository repository)
{
    public async Task<ResultT<DeviceResponse>> HandleAsync(
        UpdateDeviceCommand command,
        CancellationToken cancellationToken)
    {
        var device = await repository.GetByIdAsync(
            command.Id,
            cancellationToken);

        if (device is null)
            return ResultT<DeviceResponse>.Failure(DeviceErrors.NotFound);

        var updateResult = device.Update(
            command.Name,
            command.Brand,
            command.State);

        if (!updateResult.IsSuccess)
            return ResultT<DeviceResponse>.Failure(updateResult.Error!);

        await repository.UpdateAsync(device, cancellationToken);

        return ResultT<DeviceResponse>.Success(
            DeviceResponse.FromEntity(device));
    }
}
