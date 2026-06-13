using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Repositories;

public interface IObd2DeviceRegistrationRepository : IBaseRepository<Obd2DeviceRegistration>
{
    Task<Obd2DeviceRegistration?> FindActiveByObd2DeviceIdAsync(Obd2DeviceId obd2DeviceId, CancellationToken cancellationToken = default);
    Task<Obd2DeviceRegistration?> FindActiveByVehicleIdAsync(VehicleId vehicleId, CancellationToken cancellationToken = default);
}
