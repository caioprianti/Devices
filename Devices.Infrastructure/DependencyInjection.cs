using Devices.Application.Abstractions;
using Devices.Infrastructure.Database;
using Devices.Infrastructure.Database.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Devices.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString =
            configuration.GetConnectionString("Postgres")
            ?? throw new InvalidOperationException("Connection string 'Postgres' not found");

        services.AddDbContext<DevicesDbContext>(options =>
            options.UseNpgsql(connectionString));

        services.AddScoped<IDeviceRepository, DevicesRepository>();
        
        return services;
    }
}
