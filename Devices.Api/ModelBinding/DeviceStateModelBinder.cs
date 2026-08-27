using Devices.Domain.Enums;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Devices.Api.ModelBinding;

public sealed class DeviceStateModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        var valueResult = bindingContext.ValueProvider.GetValue(
            bindingContext.ModelName);

        if (valueResult == ValueProviderResult.None)
            return Task.CompletedTask;

        bindingContext.ModelState.SetModelValue(
            bindingContext.ModelName,
            valueResult);

        var state = valueResult.FirstValue?.ToLowerInvariant() switch
        {
            "available" => DeviceState.Available,
            "in-use" => DeviceState.InUse,
            "inactive" => DeviceState.Inactive,
            _ => (DeviceState?)null
        };

        if (state.HasValue)
        {
            bindingContext.Result = ModelBindingResult.Success(state.Value);
        }
        else
        {
            bindingContext.ModelState.TryAddModelError(
                bindingContext.ModelName,
                "State must be available, in-use, or inactive.");
        }

        return Task.CompletedTask;
    }
}
