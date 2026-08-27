using Devices.Application.Abstractions;
using Devices.Application.Devices.Common;
using Devices.Domain.Common;
using Devices.Domain.Entities;
using Devices.Domain.Errors;

namespace Devices.Application.Devices.GetAll;

public sealed class GetDevicesQueryHandler(IDeviceRepository repository)
{
    public async Task<ResultT<List<DeviceResponse>?>> HandleAsync(
        GetDevicesQuery query,
        CancellationToken cancellationToken)
    {
        var devices = await repository.GetAsync(
            query.Brand,
            query.State,
            cancellationToken);

        if (devices == null)
            return ResultT<List<DeviceResponse>?>.Success([]);

        var response = devices
            .Select(DeviceResponse.FromEntity)
            .ToList();

        return ResultT<List<DeviceResponse>?>.Success(response);
    }
}
