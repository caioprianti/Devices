using Devices.Application.Abstractions;
using Devices.Application.Devices.Create;
using Devices.Domain.Entities;
using Devices.Domain.Enums;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace Devices.UnitTests.Handlers;

public sealed class CreateDeviceCommandHandlerTests
{
    private readonly Mock<IDeviceRepository> _repositoryMock;
    private readonly CreateDeviceCommandHandler _handler;

    public CreateDeviceCommandHandlerTests()

    {
        _repositoryMock = new Mock<IDeviceRepository>();
        var loggerMock = new Mock<ILogger<CreateDeviceCommandHandler>>();
        _handler = new CreateDeviceCommandHandler(
            _repositoryMock.Object,
            loggerMock.Object);
    }
    
    
    [Fact]
    public async Task Handle_WithValidCommand_ShouldReturnSuccess()
    {
        // Arrange
        var command = new CreateDeviceCommand(
            "TestName",
            "TestBrand",
            DeviceState.Available);

        // Act
        var result = await _handler.HandleAsync(
            command,
            CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("TestName", result.Value!.Name);
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldAddDevice()
    {
        // Arrange
        var command = new CreateDeviceCommand(
            "TestName",
            "TestBrand",
            DeviceState.Available);

        // Act
        await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        _repositoryMock.Verify(
            x => x.AddAsync(
                It.IsAny<Device>(),
                CancellationToken.None),
            Times.Once);
    }
}
