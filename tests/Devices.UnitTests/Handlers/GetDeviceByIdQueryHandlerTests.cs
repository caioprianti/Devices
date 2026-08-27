using Devices.Application.Abstractions;
using Devices.Application.Devices.GetById;
using Devices.Domain.Entities;
using Devices.Domain.Enums;
using Devices.Domain.Errors;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Devices.UnitTests.Handlers;

public sealed class GetDeviceByIdQueryHandlerTests
{
    private readonly Mock<IDeviceRepository> _repositoryMock;
    private readonly Mock<ILogger<GetDeviceByIdQueryHandler>> _loggerMock;
    private readonly GetDeviceByIdQueryHandler _handler;

    public GetDeviceByIdQueryHandlerTests()
    {
        _repositoryMock = new Mock<IDeviceRepository>();
        _loggerMock = new Mock<ILogger<GetDeviceByIdQueryHandler>>();
        _handler = new GetDeviceByIdQueryHandler(
            _repositoryMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_WhenDeviceExists_ShouldReturnSuccess()
    {
        // Arrange
        var device = Device.Create(
            "TestName",
            "TestBrand",
            DeviceState.Available).Value!;

        _repositoryMock
            .Setup(x => x.GetByIdAsync(device.Id, CancellationToken.None))
            .ReturnsAsync(device);

        // Act
        var result = await _handler.HandleAsync(
            new GetDeviceByIdQuery(device.Id),
            CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(device.Id, result.Value!.Id);
    }

    [Fact]
    public async Task Handle_WhenDeviceDoesNotExist_ShouldReturnNotFound()
    {
        // Arrange
        var deviceId = Guid.NewGuid();

        _repositoryMock
            .Setup(x => x.GetByIdAsync(deviceId, CancellationToken.None))
            .ReturnsAsync((Device?)null);

        // Act
        var result = await _handler.HandleAsync(
            new GetDeviceByIdQuery(deviceId),
            CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(DeviceErrors.NotFound, result.Error);
    }
}
