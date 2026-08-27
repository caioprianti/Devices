using Devices.Domain.Entities;
using Devices.Domain.Enums;

namespace Devices.Application.Abstractions;

public interface IDeviceRepository
{
   Task<Device?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

   Task<List<Device>?> GetAsync(string? brand, DeviceState? state, CancellationToken cancellationToken);
   
   Task UpdateAsync(Device device, CancellationToken cancellationToken);

   Task AddAsync(Device device, CancellationToken cancellationToken);

   Task RemoveAsync(Device device, CancellationToken cancellationToken);
}