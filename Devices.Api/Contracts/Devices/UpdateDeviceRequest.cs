using Devices.Domain.Enums;

namespace Devices.Api.Contracts.Devices;

/// <summary>
/// Complete replacement data for a device.
/// </summary>
public sealed class UpdateDeviceRequest
{
    /// <summary>Device name.</summary>
    public required string Name { get; init; }

    /// <summary>Device brand.</summary>
    public required string Brand { get; init; }

    /// <summary>Device state.</summary>
    public required DeviceState State { get; init; }
}
