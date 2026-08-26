using Devices.Domain.Enums;

namespace Devices.Domain.Entities;

public sealed class Device
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Brand { get; set; }

    public DeviceState State { get; set; }

    public DateTime CreationTime { get; set; }
}