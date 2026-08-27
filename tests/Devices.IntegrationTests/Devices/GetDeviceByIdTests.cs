using System.Net;
using Devices.IntegrationTests.Infrastructure;

namespace Devices.IntegrationTests.Devices;

[Collection(DevicesApiCollection.Name)]
public sealed class GetDeviceByIdTests(DevicesApiFactory factory)
{
    [Fact]
    public async Task GetDeviceById_WhenDeviceExists_ShouldReturnOk()
    {
        // Arrange
        var client = factory.CreateClient();
        var deviceId = await DeviceTestData.CreateAsync(client);

        // Act
        var response = await client.GetAsync(
            $"/devices/{deviceId}");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }

    [Fact]
    public async Task GetDeviceById_WhenDeviceDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var client = factory.CreateClient();
        var deviceId = Guid.NewGuid();

        // Act
        var response = await client.GetAsync(
            $"/devices/{deviceId}");

        // Assert
        Assert.Equal(
            HttpStatusCode.NotFound,
            response.StatusCode);
    }
}
