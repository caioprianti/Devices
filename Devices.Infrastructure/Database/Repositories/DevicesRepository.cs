using Devices.Application.Abstractions;
using Devices.Domain.Entities;
using Devices.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Devices.Infrastructure.Database.Repositories;

public class DevicesRepository(DevicesDbContext context) : IDeviceRepository
{

    public async Task<List<Device>?> GetAllAsync(CancellationToken cancellationToken)
    {
        return await context.Devices
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
    
    public async Task<Device?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await context.Devices
            .FirstOrDefaultAsync(
                x => x.Id == id,
                cancellationToken);
    }

    public async Task<List<Device>?> GetByBrandAsync(string brand, CancellationToken cancellationToken)
    {
        return await context.Devices
            .AsNoTracking()
            .Where(x => x.Brand == brand)
            .ToListAsync(cancellationToken);

    }

    public async Task<List<Device>?> GetByStateAsync(DeviceState state, CancellationToken cancellationToken)
    {
        return await context.Devices
            .AsNoTracking()
            .Where(x => x.State == state)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateAsync(Device device, CancellationToken cancellationToken)
    {
        context.Devices.Update(device);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task AddAsync(Device device, CancellationToken cancellationToken)
    {
        context.Devices.Add(device);
        await context.SaveChangesAsync(cancellationToken);
    }
    
    public async Task Remove(Device device, CancellationToken cancellationToken)
    {
        context.Devices.Remove(device);
        await context.SaveChangesAsync(cancellationToken);
    }
}