using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.IoT.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace atelier_platform_aplicaciones_web.IoT.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class Obd2DeviceRegistrationRepository(AppDbContext context) : BaseRepository<Obd2DeviceRegistration>(context), IObd2DeviceRegistrationRepository
{
    public async Task<Obd2DeviceRegistration?> FindActiveByObd2DeviceIdAsync(Obd2DeviceId obd2DeviceId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Obd2DeviceRegistration>()
            .FirstOrDefaultAsync(r => r.Obd2DeviceId == obd2DeviceId && r.Status == Obd2RegistrationStatus.Active, cancellationToken);
    }

    public async Task<Obd2DeviceRegistration?> FindActiveByVehicleIdAsync(VehicleId vehicleId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Obd2DeviceRegistration>()
            .FirstOrDefaultAsync(r => r.VehicleId == vehicleId && r.Status == Obd2RegistrationStatus.Active, cancellationToken);
    }
}
