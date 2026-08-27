using Devices.Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Testcontainers.PostgreSql;

namespace Devices.IntegrationTests.Infrastructure;

public sealed class DevicesApiFactory
    : WebApplicationFactory<Program>,
        IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgresContainer =
        new PostgreSqlBuilder()
            .WithImage("postgres:17-alpine")
            .WithDatabase("devices-tests")
            .WithUsername("postgres")
            .WithPassword("postgres")
            .Build();

    public async Task InitializeAsync()
    {
        await _postgresContainer.StartAsync();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Testing");

        builder.UseSetting(
            "ConnectionStrings:Postgres",
            _postgresContainer.GetConnectionString());
    }

    public new async Task DisposeAsync()
    {
        await _postgresContainer.DisposeAsync();

        await base.DisposeAsync();
    }
}