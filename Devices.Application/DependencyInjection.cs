using Devices.Application.Devices.Create;
using Devices.Application.Devices.Delete;
using Devices.Application.Devices.GetAll;
using Devices.Application.Devices.GetById;
using Devices.Application.Devices.Patch;
using Devices.Application.Devices.Update;
using Microsoft.Extensions.DependencyInjection;

namespace Devices.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<CreateDeviceCommandHandler>();
        services.AddScoped<DeleteDeviceCommandHandler>();
        services.AddScoped<GetDevicesQueryHandler>();
        services.AddScoped<GetDeviceByIdQueryHandler>();
        services.AddScoped<PatchDeviceCommandHandler>();
        services.AddScoped<UpdateDeviceCommandHandler>();

        return services;
    }
}
