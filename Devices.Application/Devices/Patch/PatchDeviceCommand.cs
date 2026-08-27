using Devices.Domain.Enums;

namespace Devices.Application.Devices.Patch;

public sealed record PatchDeviceCommand(
    Guid Id,
    string? Name,
    string? Brand,
    DeviceState? State);
