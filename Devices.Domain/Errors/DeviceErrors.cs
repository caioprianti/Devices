using Devices.Domain.Common;

namespace Devices.Domain.Errors;

public static class DeviceErrors
{
    public static readonly Error NotFound =
        new(
            "Device.NotFound",
            "The device was not found.");

    public static readonly Error CannotUpdateInUse =
        new(
            "Device.CannotUpdateInUse",
            "Name and brand cannot be changed while the device is in use.");

    public static readonly Error CannotDeleteInUse =
        new(
            "Device.CannotDeleteInUse",
            "A device in use cannot be deleted.");
}