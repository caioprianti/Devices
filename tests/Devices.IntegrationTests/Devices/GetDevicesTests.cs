using System.Net;
using Devices.IntegrationTests.Infrastructure;

namespace Devices.IntegrationTests.Devices;

[Collection(DevicesApiCollection.Name)]
public sealed class GetDevicesTests(DevicesApiFactory factory)
{
    [Fact]
    public async Task GetDevices_ShouldReturnOk()
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync(
            "/devices");

        // Assert
        Assert.Equal(
            HttpStatusCode.OK,
            response.StatusCode);
    }

    [Fact]
    public async Task GetDevices_WithInvalidState_ShouldReturnBadRequest()
    {
        // Arrange
        var client = factory.CreateClient();

        // Act
        var response = await client.GetAsync(
            "/devices?state=invalid");

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }
}
