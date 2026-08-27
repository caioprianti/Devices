using Devices.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Devices.Api.Extensions;

public static class MigrationExtensions
{
    public static async Task ApplyMigrationsAsync(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();

        var dbContext = scope.ServiceProvider
            .GetRequiredService<DevicesDbContext>();

        await dbContext.Database.MigrateAsync();
    }
}