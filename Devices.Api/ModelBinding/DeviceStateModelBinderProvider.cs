using Devices.Domain.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Devices.Api.ModelBinding;

public sealed class DeviceStateModelBinderProvider : IModelBinderProvider
{
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        ArgumentNullException.ThrowIfNull(context);

        var modelType = Nullable.GetUnderlyingType(context.Metadata.ModelType)
                        ?? context.Metadata.ModelType;

        return modelType == typeof(DeviceState)
            ? new DeviceStateModelBinder()
            : null;
    }
}
