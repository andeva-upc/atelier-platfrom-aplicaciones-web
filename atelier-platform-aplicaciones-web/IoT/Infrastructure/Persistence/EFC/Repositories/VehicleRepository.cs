using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EFC.Configuration;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace atelier_platform_aplicaciones_web.IoT.Infrastructure.Persistence.EFC.Repositories;

public class VehicleRepository : BaseRepository<Vehicle>, IVehicleRepository
{
    public VehicleRepository(AppDbContext context) : base(context) {}

    public async Task<Vehicle?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Vehicle>().FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
    }

    public async Task<Vehicle?> FindByVinAsync(string vin, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Vehicle>().FirstOrDefaultAsync(v => v.Vin == vin, cancellationToken);
    }
}
