using Devices.Domain.Enums;

namespace Devices.Api.Contracts.Devices;

/// <summary>
/// Optional filters used to retrieve devices.
/// </summary>
public sealed class GetDevicesRequest
{
    /// <summary>Exact brand used to filter devices.</summary>
    public string? Brand { get; init; }

    /// <summary>State used to filter devices.</summary>
    public DeviceState? State { get; init; }
}
