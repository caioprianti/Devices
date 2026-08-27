using Devices.Application.Abstractions;
using Devices.Application.Devices.Common;
using Devices.Domain.Common;
using Devices.Domain.Entities;
using Devices.Domain.Errors;
using Microsoft.Extensions.Logging;

namespace Devices.Application.Devices.GetAll;

public sealed class GetDevicesQueryHandler(
    IDeviceRepository repository,
    ILogger<GetDevicesQueryHandler> logger)
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
        {
            logger.LogDebug(
                "[GetDevicesQueryHandler] - No devices were retrieved for brand {Brand} and state {State}",
                query.Brand,
                query.State);

            return ResultT<List<DeviceResponse>?>.Success([]);
        }

        var response = devices
            .Select(DeviceResponse.FromEntity)
            .ToList();

        logger.LogDebug(
            "[GetDevicesQueryHandler] - Retrieved {DeviceCount} devices for brand {Brand} and state {State}",
            response.Count,
            query.Brand,
            query.State);

        return ResultT<List<DeviceResponse>?>.Success(response);
    }
}
