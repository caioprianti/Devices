using System.Net;
using System.Net.Http.Json;
using Devices.IntegrationTests.Infrastructure;

namespace Devices.IntegrationTests.Devices;

[Collection(DevicesApiCollection.Name)]
public sealed class CreateDeviceTests(DevicesApiFactory factory)
{
    [Fact]
    public async Task CreateDevice_WithValidRequest_ShouldReturnCreated()
    {
        // Arrange
        var client = factory.CreateClient();
        var request = new
        {
            name = "Router X1",
            brand = "Acme",
            state = "available"
        };

        // Act
        var response = await client.PostAsJsonAsync(
            "/devices",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.Created,
            response.StatusCode);
    }

    [Fact]
    public async Task CreateDevice_WithEmptyName_ShouldReturnBadRequest()
    {
        // Arrange
        var client = factory.CreateClient();
        var request = new
        {
            name = string.Empty,
            brand = "Acme",
            state = "available"
        };

        // Act
        var response = await client.PostAsJsonAsync(
            "/devices",
            request);

        // Assert
        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }
}
