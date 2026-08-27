using System.Net;
using System.Net.Http.Json;
using Devices.IntegrationTests.Infrastructure;

namespace Devices.IntegrationTests.Devices;

[Collection(DevicesApiCollection.Name)]
public sealed class UpdateDeviceTests(DevicesApiFactory factory)
{
    [Fact]
    public async Task UpdateDevice_WhenDeviceExists_ShouldReturnOk()
    {
        // Arrange
        var client = factory.CreateClient();
        var deviceId = await DeviceTestData.CreateAsync(client);
        var request = new
        {
            name = "Updated device",
            brand = "Updated brand",
            state = "inactive"
        };

        // Act
        var response = await client.PutAsJsonAsync(
            $"/devices/{deviceId}",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }

    [Fact]
    public async Task UpdateDevice_WhenDeviceDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var client = factory.CreateClient();
        var deviceId = Guid.NewGuid();
        var request = new
        {
            name = "Updated device",
            brand = "Updated brand",
            state = "inactive"
        };

        // Act
        var response = await client.PutAsJsonAsync(
            $"/devices/{deviceId}",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }
}
