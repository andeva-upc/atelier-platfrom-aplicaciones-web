using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Repositories;

public interface IVehicleRepository : IBaseRepository<Vehicle>
{
    Task<Vehicle?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Vehicle?> FindByVinAsync(string vin, CancellationToken cancellationToken = default);
}
