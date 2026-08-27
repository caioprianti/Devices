using Devices.Domain.Common;
using Devices.Domain.Enums;
using Devices.Domain.Errors;

namespace Devices.Domain.Entities;

public sealed class Device
{
    public Guid Id { get; private set; }

    public string Name { get; private set; }

    public string Brand { get; private set; }

    public DeviceState State { get; private set; }

    public DateTime CreationTime { get; private set; }

    private Device() { }

    private Device(
        Guid id,
        string name,
        string brand,
        DeviceState state,
        DateTime creationTime)
    {
        Id = id;
        Name = name;
        Brand = brand;
        State = state;
        CreationTime = creationTime;
    }

    public static ResultT<Device> Create(
        string name,
        string brand,
        DeviceState state)
    {
        var device = new Device(
            Guid.NewGuid(),
            name,
            brand,
            state,
            DateTime.UtcNow);

        return ResultT<Device>.Success(device);
    }

    public Result Update(string? name, string? brand, DeviceState? state)
    {
        if (State == DeviceState.InUse)
            return Result.Failure(
                DeviceErrors.CannotUpdateInUse);

        if (!string.IsNullOrEmpty(name))
            Name = name;
        
        if (!string.IsNullOrEmpty(brand))
            Brand = brand;
        
        if (state.HasValue)
            State = state.Value;

        return Result.Success();
    }

    public Result ChangeState(DeviceState state)
    {
        State = state;

        return Result.Success();
    }

    public bool CanBeDeleted()
        => State != DeviceState.InUse;
}