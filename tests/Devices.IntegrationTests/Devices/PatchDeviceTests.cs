using System.Net;
using System.Net.Http.Json;
using Devices.IntegrationTests.Infrastructure;

namespace Devices.IntegrationTests.Devices;

[Collection(DevicesApiCollection.Name)]
public sealed class PatchDeviceTests(DevicesApiFactory factory)
{
    [Fact]
    public async Task PatchDevice_WhenDeviceExists_ShouldReturnOk()
    {
        // Arrange
        var client = factory.CreateClient();
        var deviceId = await DeviceTestData.CreateAsync(client);
        var request = new
        {
            name = "Patched device"
        };

        // Act
        var response = await client.PatchAsJsonAsync(
            $"/devices/{deviceId}",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }

    [Fact]
    public async Task PatchDevice_WhenDeviceDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var client = factory.CreateClient();
        var deviceId = Guid.NewGuid();
        var request = new
        {
            name = "Patched device"
        };

        // Act
        var response = await client.PatchAsJsonAsync(
            $"/devices/{deviceId}",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }
}
