namespace Devices.IntegrationTests.Infrastructure;

[CollectionDefinition(Name)]
public sealed class DevicesApiCollection : ICollectionFixture<DevicesApiFactory>
{
    public const string Name = "Devices API";
}
