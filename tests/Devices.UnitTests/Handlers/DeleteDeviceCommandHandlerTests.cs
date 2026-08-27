using Devices.Application.Abstractions;
using Devices.Application.Devices.Delete;
using Devices.Domain.Entities;
using Devices.Domain.Enums;
using Devices.Domain.Errors;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Devices.UnitTests.Handlers;

public sealed class DeleteDeviceCommandHandlerTests
{
    private readonly Mock<IDeviceRepository> _repositoryMock;
    private readonly DeleteDeviceCommandHandler _handler;

    public DeleteDeviceCommandHandlerTests()

    {
        _repositoryMock = new Mock<IDeviceRepository>();
        var loggerMock = new Mock<ILogger<DeleteDeviceCommandHandler>>();
        _handler = new DeleteDeviceCommandHandler(
            _repositoryMock.Object,
            loggerMock.Object);
    }
    [Fact]
    public async Task Handle_WhenDeviceIsAvailable_ShouldReturnSuccess()
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
            new DeleteDeviceCommand(device.Id),
            CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _repositoryMock.Verify(
            x => x.RemoveAsync(device, CancellationToken.None),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenDeviceIsInUse_ShouldReturnFailure()
    {
        // Arrange
        var device = Device.Create(
            "TestName",
            "TestBrand",
            DeviceState.InUse).Value!;

        _repositoryMock
            .Setup(x => x.GetByIdAsync(device.Id, CancellationToken.None))
            .ReturnsAsync(device);

        // Act
        var result = await _handler.HandleAsync(
            new DeleteDeviceCommand(device.Id),
            CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(DeviceErrors.CannotDeleteInUse, result.Error);
        _repositoryMock.Verify(
            x => x.RemoveAsync(
                It.IsAny<Device>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
