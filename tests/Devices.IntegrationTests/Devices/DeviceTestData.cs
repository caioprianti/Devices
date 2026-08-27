using System.Net.Http.Json;

namespace Devices.IntegrationTests.Devices;

internal static class DeviceTestData
{
    public static async Task<Guid> CreateAsync(
        HttpClient client,
        string state = "available")
    {
        var response = await client.PostAsJsonAsync(
            "/devices",
            new
            {
                name = "Test device",
                brand = $"Test brand {Guid.NewGuid():N}",
                state
            });

        response.EnsureSuccessStatusCode();

        var device = await response.Content
            .ReadFromJsonAsync<CreatedDeviceResponse>();

        return device!.Id;
    }

    private sealed record CreatedDeviceResponse(Guid Id);
}
