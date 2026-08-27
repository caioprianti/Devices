using System.Net;
using Devices.IntegrationTests.Infrastructure;

namespace Devices.IntegrationTests.Devices;

[Collection(DevicesApiCollection.Name)]
public sealed class DeleteDeviceTests(DevicesApiFactory factory)
{
    [Fact]
    public async Task DeleteDevice_WhenDeviceIsAvailable_ShouldReturnNoContent()
    {
        // Arrange
        var client = factory.CreateClient();
        var deviceId = await DeviceTestData.CreateAsync(client);

        // Act
        var response = await client.DeleteAsync(
            $"/devices/{deviceId}");

        // Assert
        Assert.Equal(
            HttpStatusCode.NoContent,
            response.StatusCode);
    }

    [Fact]
    public async Task DeleteDevice_WhenDeviceIsInUse_ShouldReturnConflict()
    {
        // Arrange
        var client = factory.CreateClient();
        var deviceId = await DeviceTestData.CreateAsync(
            client,
            "in-use");

        // Act
        var response = await client.DeleteAsync(
            $"/devices/{deviceId}");

        // Assert
        Assert.Equal(
            HttpStatusCode.Conflict,
            response.StatusCode);
    }
}
