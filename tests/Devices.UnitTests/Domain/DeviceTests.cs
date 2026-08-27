using Devices.Domain.Entities;
using Devices.Domain.Enums;
using Devices.Domain.Errors;

namespace Devices.UnitTests.Domain;

public sealed class DeviceTests
{
    [Fact]
    public void Create_ShouldCreateDeviceWithProvidedValues()
    {
        // Act
        var result = Device.Create(
            "TestName",
            "TestBrand",
            DeviceState.Available);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotEqual(Guid.Empty, result.Value!.Id);
        Assert.Equal("TestName", result.Value.Name);
        Assert.Equal("TestBrand", result.Value.Brand);
        Assert.Equal(DeviceState.Available, result.Value.State);
    }

    [Fact]
    public void Update_WhenDeviceIsAvailable_ShouldUpdateDevice()
    {
        // Arrange
        var device = Device.Create(
            "TestName",
            "TestBrand",
            DeviceState.Available).Value!;

        // Act
        var result = device.Update(
            "Updated name",
            "Updated brand",
            DeviceState.Inactive);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("Updated name", device.Name);
        Assert.Equal("Updated brand", device.Brand);
        Assert.Equal(DeviceState.Inactive, device.State);
    }

    [Fact]
    public void Patch_WhenDeviceIsInUseAndNameChanges_ShouldReturnFailure()
    {
        // Arrange
        var device = Device.Create(
            "TestName",
            "TestBrand",
            DeviceState.InUse).Value!;

        // Act
        var result = device.Patch(
            "Updated name",
            null,
            null);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(DeviceErrors.CannotUpdateInUse, result.Error);
        Assert.Equal("TestName", device.Name);
    }

    [Fact]
    public void Patch_WhenDeviceIsInUseAndOnlyStateChanges_ShouldReturnSuccess()
    {
        // Arrange
        var device = Device.Create(
            "TestName",
            "TestBrand",
            DeviceState.InUse).Value!;

        // Act
        var result = device.Patch(
            null,
            null,
            DeviceState.Inactive);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(DeviceState.Inactive, device.State);
    }

    [Fact]
    public void CanBeDeleted_WhenDeviceIsInUse_ShouldReturnFalse()
    {
        // Arrange
        var device = Device.Create(
            "TestName",
            "TestBrand",
            DeviceState.InUse).Value!;

        // Act
        var canBeDeleted = device.CanBeDeleted();

        // Assert
        Assert.False(canBeDeleted);
    }

    [Fact]
    public void Update_ShouldNotChangeCreationTime()
    {
        // Arrange
        var device = Device.Create(
            "TestName",
            "TestBrand",
            DeviceState.Available).Value!;
        var creationTime = device.CreationTime;

        // Act
        device.Update(
            "Updated name",
            "Updated brand",
            DeviceState.Inactive);

        // Assert
        Assert.Equal(creationTime, device.CreationTime);
    }
}
