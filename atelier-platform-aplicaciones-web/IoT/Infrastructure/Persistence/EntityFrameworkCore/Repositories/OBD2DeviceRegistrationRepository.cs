using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.IoT.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace atelier_platform_aplicaciones_web.IoT.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class OBD2DeviceRegistrationRepository : BaseRepository<OBD2DeviceRegistration>, IOBD2DeviceRegistrationRepository
{
    public OBD2DeviceRegistrationRepository(AppDbContext context) : base(context) {}

    public async Task<OBD2DeviceRegistration?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<OBD2DeviceRegistration>().FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<OBD2DeviceRegistration?> FindByObd2DeviceIdAndStatusAsync(Guid obd2DeviceId, OBD2DeviceRegistrationStatus status, CancellationToken cancellationToken = default)
    {
        return await Context.Set<OBD2DeviceRegistration>()
            .FirstOrDefaultAsync(r => r.Obd2DeviceId == obd2DeviceId && r.Status == status, cancellationToken);
    }

    public async Task<OBD2DeviceRegistration?> FindByVehicleIdAndStatusAsync(VehicleId vehicleId, OBD2DeviceRegistrationStatus status, CancellationToken cancellationToken = default)
    {
        return await Context.Set<OBD2DeviceRegistration>()
            .FirstOrDefaultAsync(r => r.VehicleId == vehicleId && r.Status == status, cancellationToken);
    }

    public async Task<IEnumerable<OBD2DeviceRegistration>> FindByBranchIdAsync(BranchId branchId, OBD2DeviceRegistrationStatus status, CancellationToken cancellationToken = default)
    {
        return await Context.Set<OBD2DeviceRegistration>()
            .Where(r => r.BranchId == branchId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<OBD2DeviceRegistration>> FindByBranchIdAsync(BranchId branchId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<OBD2DeviceRegistration>()
            .Where(r => r.BranchId == branchId)
            .ToListAsync(cancellationToken);
    }
}
