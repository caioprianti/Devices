using Devices.Application.Abstractions;
using Devices.Application.Devices.Common;
using Devices.Domain.Common;
using Devices.Domain.Errors;

namespace Devices.Application.Devices.GetById;

public sealed class GetDeviceByIdQueryHandler(IDeviceRepository repository)
{
    public async Task<ResultT<DeviceResponse>> HandleAsync(
        GetDeviceByIdQuery query,
        CancellationToken cancellationToken)
    {
        var device = await repository.GetByIdAsync(
            query.Id,
            cancellationToken);

        return device is null
            ? ResultT<DeviceResponse>.Failure(DeviceErrors.NotFound)
            : ResultT<DeviceResponse>.Success(DeviceResponse.FromEntity(device));
    }
}
