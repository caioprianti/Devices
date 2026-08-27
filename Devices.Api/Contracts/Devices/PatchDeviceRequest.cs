using Devices.Domain.Enums;

namespace Devices.Api.Contracts.Devices;

/// <summary>
/// Fields to update on a device.
/// </summary>
public sealed class PatchDeviceRequest
{
    /// <summary>New device name.</summary>
    public string? Name { get; init; }

    /// <summary>New device brand.</summary>
    public string? Brand { get; init; }

    /// <summary>New device state.</summary>
    public DeviceState? State { get; init; }
}
