using Devices.Domain.Enums;

namespace Devices.Application.Devices.GetAll;

public sealed record GetDevicesQuery(
    string? Brand,
    DeviceState? State);
