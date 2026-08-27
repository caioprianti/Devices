using Devices.Domain.Enums;

namespace Devices.Application.Devices.Create;

public sealed record CreateDeviceCommand(
    string Name,
    string Brand,
    DeviceState State);
