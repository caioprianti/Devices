using Devices.Domain.Enums;

namespace Devices.Api.Contracts.Devices;

/// <summary>
/// Data required to create a device.
/// </summary>
public sealed class CreateDeviceRequest
{
    /// <summary>Device name.</summary>
    public required string Name { get; init; }

    /// <summary>Device brand.</summary>
    public required string Brand { get; init; }

    /// <summary>Initial device state.</summary>
    public required DeviceState State { get; init; }
}
