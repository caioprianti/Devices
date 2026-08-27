using Devices.Application.Abstractions;
using Devices.Application.Devices.Common;
using Devices.Domain.Common;
using Devices.Domain.Errors;
using Microsoft.Extensions.Logging;

namespace Devices.Application.Devices.GetById;

public sealed class GetDeviceByIdQueryHandler(
    IDeviceRepository repository,
    ILogger<GetDeviceByIdQueryHandler> logger)
{
    public async Task<ResultT<DeviceResponse>> HandleAsync(
        GetDeviceByIdQuery query,
        CancellationToken cancellationToken)
    {
        var device = await repository.GetByIdAsync(
            query.Id,
            cancellationToken);

        if (device is null)
        {
            logger.LogWarning(
                "[GetDeviceByIdQueryHandler] - Device {DeviceId} was not found",
                query.Id);

            return ResultT<DeviceResponse>.Failure(DeviceErrors.NotFound);
        }

        logger.LogDebug(
            "[GetDeviceByIdQueryHandler] - Device {DeviceId} was retrieved",
            device.Id);

        return ResultT<DeviceResponse>.Success(
            DeviceResponse.FromEntity(device));
    }
}
