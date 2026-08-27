using Devices.Application.Abstractions;
using Devices.Domain.Entities;
using Devices.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Devices.Infrastructure.Database.Repositories;

public class DevicesRepository(DevicesDbContext context) : IDeviceRepository
{
    public async Task<List<Device>?> GetAsync(string? brand, DeviceState? state, CancellationToken cancellationToken)
    {
        var query = context.Devices
            .AsNoTracking();
        
        if (!string.IsNullOrEmpty(brand))
            query = query.Where(x => x.Brand == brand);
        
        if (state != null)
            query = query.Where(x => x.State == state);
        
        return await query
            .OrderBy(x => x.CreationTime)
            .ToListAsync(cancellationToken);
    }

    public async Task<Device?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Devices
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task UpdateAsync(Device device, CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddAsync(Device device, CancellationToken cancellationToken)
    {
        context.Devices.Add(device);
        await context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task RemoveAsync(Device device, CancellationToken cancellationToken)
    {
        context.Devices.Remove(device);
        await context.SaveChangesAsync(cancellationToken);
    }
}
