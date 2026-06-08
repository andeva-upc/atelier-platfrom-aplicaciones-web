using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Repositories;

public interface IVehicleRegistrationRepository : IBaseRepository<VehicleRegistration>
{
    Task<VehicleRegistration?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<VehicleRegistration?> FindByVehicleIdAndStatusAsync(Guid vehicleId, VehicleRegistrationStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<VehicleRegistration>> FindActiveByBranchIdAsync(Guid branchId, CancellationToken cancellationToken = default);
    Task<IEnumerable<VehicleRegistration>> FindActiveByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
}
