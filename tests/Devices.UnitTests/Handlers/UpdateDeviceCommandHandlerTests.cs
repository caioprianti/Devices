using Devices.Application.Abstractions;
using Devices.Application.Devices.Update;
using Devices.Domain.Entities;
using Devices.Domain.Enums;
using Devices.Domain.Errors;
using Microsoft.Extensions.Logging;
using Moq;

namespace Devices.UnitTests.Handlers;

public sealed class UpdateDeviceCommandHandlerTests
{
    private readonly Mock<IDeviceRepository> _repositoryMock;
    private readonly Mock<ILogger<UpdateDeviceCommandHandler>> _loggerMock;
    private readonly UpdateDeviceCommandHandler _handler;

    public UpdateDeviceCommandHandlerTests()
    {
        _repositoryMock = new Mock<IDeviceRepository>();
        _loggerMock = new Mock<ILogger<UpdateDeviceCommandHandler>>();
        _handler = new UpdateDeviceCommandHandler(
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
            new UpdateDeviceCommand(
                device.Id,
                "Updated name",
                "Updated brand",
                DeviceState.Inactive),
            CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Updated name", result.Value!.Name);
        _repositoryMock.Verify(
            x => x.UpdateAsync(device, CancellationToken.None),
            Times.Once);
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
            new UpdateDeviceCommand(
                deviceId,
                "TestName",
                "TestBrand",
                DeviceState.Available),
            CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(DeviceErrors.NotFound, result.Error);
    }
}
