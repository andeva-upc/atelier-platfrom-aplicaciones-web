using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.IoT.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EFC.Configuration;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace atelier_platform_aplicaciones_web.IoT.Infrastructure.Persistence.EFC.Repositories;

public class VehicleRegistrationRepository : BaseRepository<VehicleRegistration>, IVehicleRegistrationRepository
{
    public VehicleRegistrationRepository(AppDbContext context) : base(context) {}

    public async Task<VehicleRegistration?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<VehicleRegistration>().FirstOrDefaultAsync(r => r.Id == id, cancellationToken);
    }

    public async Task<VehicleRegistration?> FindByVehicleIdAndStatusAsync(Guid vehicleId, VehicleRegistrationStatus status, CancellationToken cancellationToken = default)
    {
        return await Context.Set<VehicleRegistration>()
            .FirstOrDefaultAsync(r => r.VehicleId == vehicleId && r.Status == status, cancellationToken);
    }

    public async Task<IEnumerable<VehicleRegistration>> FindActiveByBranchIdAsync(Guid branchId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<VehicleRegistration>()
            .FromSqlRaw(@"
                SELECT vr.* 
                FROM vehicle_registrations vr
                JOIN customers c ON vr.user_id = c.user_id
                JOIN customer_registrations cr ON c.id = cr.customer_id
                WHERE cr.branch_id = {0} 
                  AND cr.status = 'ACTIVE' 
                  AND vr.status = 'ACTIVE' 
                  AND vr.deleted_at IS NULL", branchId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<VehicleRegistration>> FindActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<VehicleRegistration>()
            .Where(r => r.UserId == userId && r.Status == VehicleRegistrationStatus.ACTIVE && r.DeletedAt == null)
            .ToListAsync(cancellationToken);
    }
}
