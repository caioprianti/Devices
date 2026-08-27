using Devices.Application.Abstractions;
using Devices.Application.Devices.Common;
using Devices.Domain.Common;
using Devices.Domain.Errors;

namespace Devices.Application.Devices.Patch;

public sealed class PatchDeviceCommandHandler(IDeviceRepository repository)
{
    public async Task<ResultT<DeviceResponse>> HandleAsync(
        PatchDeviceCommand command,
        CancellationToken cancellationToken)
    {
        var device = await repository.GetByIdAsync(
            command.Id,
            cancellationToken);

        if (device is null)
            return ResultT<DeviceResponse>.Failure(DeviceErrors.NotFound);

        var patchResult = device.Update(
            command.Name,
            command.Brand,
            command.State);

        if (!patchResult.IsSuccess)
            return ResultT<DeviceResponse>.Failure(patchResult.Error!);

        await repository.UpdateAsync(device, cancellationToken);

        return ResultT<DeviceResponse>.Success(
            DeviceResponse.FromEntity(device));
    }
}
