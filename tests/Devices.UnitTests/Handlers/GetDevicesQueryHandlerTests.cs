using Devices.Application.Abstractions;
using Devices.Application.Devices.GetAll;
using Devices.Domain.Entities;
using Devices.Domain.Enums;
using Microsoft.Extensions.Logging;
using Moq;

namespace Devices.UnitTests.Handlers;

public sealed class GetDevicesQueryHandlerTests
{
    private readonly Mock<IDeviceRepository> _repositoryMock;
    private readonly Mock<ILogger<GetDevicesQueryHandler>> _loggerMock;
    private readonly GetDevicesQueryHandler _handler;

    public GetDevicesQueryHandlerTests()
    {
        _repositoryMock = new Mock<IDeviceRepository>();
        _loggerMock = new Mock<ILogger<GetDevicesQueryHandler>>();
        _handler = new GetDevicesQueryHandler(
            _repositoryMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WithDevices_ShouldReturnDevices()
    {
        // Arrange
        var device = Device.Create(
            "TestName",
            "TestBrand",
            DeviceState.Available).Value!;

        _repositoryMock
            .Setup(x => x.GetAsync(null, null, CancellationToken.None))
            .ReturnsAsync([device]);

        // Act
        var result = await _handler.HandleAsync(
            new GetDevicesQuery(null, null),
            CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value!);
    }

    [Fact]
    public async Task Handle_WithoutDevices_ShouldReturnEmptyList()
    {
        // Arrange
        _repositoryMock
            .Setup(x => x.GetAsync(null, null, CancellationToken.None))
            .ReturnsAsync((List<Device>?)null);

        // Act
        var result = await _handler.HandleAsync(
            new GetDevicesQuery(null, null),
            CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value!);
    }
}
