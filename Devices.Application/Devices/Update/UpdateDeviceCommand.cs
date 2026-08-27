using Devices.Domain.Enums;

namespace Devices.Application.Devices.Update;

public sealed record UpdateDeviceCommand(
    Guid Id,
    string Name,
    string Brand,
    DeviceState State);
