using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Repositories;

public interface IOBD2DeviceRegistrationRepository : IBaseRepository<OBD2DeviceRegistration>
{
    Task<OBD2DeviceRegistration?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<OBD2DeviceRegistration?> FindByObd2DeviceIdAndStatusAsync(Guid obd2DeviceId, OBD2DeviceRegistrationStatus status, CancellationToken cancellationToken = default);
    Task<OBD2DeviceRegistration?> FindByVehicleIdAndStatusAsync(VehicleId vehicleId, OBD2DeviceRegistrationStatus status, CancellationToken cancellationToken = default);
    Task<IEnumerable<OBD2DeviceRegistration>> FindByBranchIdAsync(BranchId branchId, CancellationToken cancellationToken = default);
}
