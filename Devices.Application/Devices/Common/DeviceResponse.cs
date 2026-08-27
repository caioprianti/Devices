using Devices.Domain.Entities;
using Devices.Domain.Enums;

namespace Devices.Application.Devices.Common;

public sealed record DeviceResponse(
    Guid Id,
    string Name,
    string Brand,
    DeviceState State,
    DateTime CreationTime)
{
    public static DeviceResponse FromEntity(Device device)
        => new(
            device.Id,
            device.Name,
            device.Brand,
            device.State,
            device.CreationTime);
}
