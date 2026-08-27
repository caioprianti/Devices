using Devices.Application.Abstractions;
using Devices.Application.Devices.Patch;
using Devices.Domain.Entities;
using Devices.Domain.Enums;
using Devices.Domain.Errors;
using Microsoft.Extensions.Logging;
using Moq;

namespace Devices.UnitTests.Handlers;

public sealed class PatchDeviceCommandHandlerTests
{
    private readonly Mock<IDeviceRepository> _repositoryMock;
    private readonly Mock<ILogger<PatchDeviceCommandHandler>> _loggerMock;
    private readonly PatchDeviceCommandHandler _handler;

    public PatchDeviceCommandHandlerTests()
    {
        _repositoryMock = new Mock<IDeviceRepository>();
        _loggerMock = new Mock<ILogger<PatchDeviceCommandHandler>>();
        _handler = new PatchDeviceCommandHandler(
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
            new PatchDeviceCommand(
                device.Id,
                "Patched name",
                null,
                null),
            CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Patched name", result.Value!.Name);
        _repositoryMock.Verify(
            x => x.UpdateAsync(device, CancellationToken.None),
            Times.Once);
    }

    [Fact]
    public async Task Handle_WhenInUseNameChanges_ShouldReturnFailure()
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
            new PatchDeviceCommand(
                device.Id,
                "Patched name",
                null,
                null),
            CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(DeviceErrors.CannotUpdateInUse, result.Error);
        _repositoryMock.Verify(
            x => x.UpdateAsync(
                It.IsAny<Device>(),
                It.IsAny<CancellationToken>()),
            Times.Never);
    }
}
