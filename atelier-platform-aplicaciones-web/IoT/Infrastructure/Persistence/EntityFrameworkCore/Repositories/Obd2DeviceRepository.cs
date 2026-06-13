using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.IoT.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace atelier_platform_aplicaciones_web.IoT.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class Obd2DeviceRepository(AppDbContext context) : BaseRepository<Obd2Device>(context), IObd2DeviceRepository
{
    public async Task<Obd2Device?> FindObd2DeviceByIdAsync(Obd2DeviceId id, CancellationToken cancellationToken)
    {
        return await Context.Set<Obd2Device>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Obd2Device>> FindAllByBranchIdAsync(BranchId branchId, CancellationToken cancellationToken)
    {
        return await Context.Set<Obd2Device>()
            .Where(e => e.BranchId == branchId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Obd2Device>> FindAvailableByBranchIdAsync(BranchId branchId, CancellationToken cancellationToken)
    {
        return await Context.Set<Obd2Device>()
            .Where(e => e.BranchId == branchId && e.Status == Obd2DeviceStatus.Available)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByIdAsync(Obd2DeviceId id, CancellationToken cancellationToken)
    {
        return await Context.Set<Obd2Device>().AnyAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByMacAddressAsync(string macAddress, CancellationToken cancellationToken)
    {
        return await Context.Set<Obd2Device>().AnyAsync(e => e.MacAddress == macAddress, cancellationToken);
    }

    // Explicit Soft Delete implementation
    void IBaseRepository<Obd2Device>.Remove(Obd2Device entity)
    {
        entity.DeletedAt = DateTimeOffset.UtcNow;
        Update(entity);
    }
}
