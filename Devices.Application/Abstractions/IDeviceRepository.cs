using Devices.Domain.Entities;
using Devices.Domain.Enums;

namespace Devices.Application.Abstractions;

public interface IDeviceRepository
{
   Task<Device?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

   Task<List<Device>?> GetAllAsync(CancellationToken cancellationToken);

   Task<List<Device>?> GetByBrandAsync(string brand, CancellationToken cancellationToken);

   Task<List<Device>?> GetByStateAsync(DeviceState state, CancellationToken cancellationToken);
   
   Task UpdateAsync(Device device, CancellationToken cancellationToken);

   Task AddAsync(Device device, CancellationToken cancellationToken);

   Task Remove(Device device, CancellationToken cancellationToken);
}