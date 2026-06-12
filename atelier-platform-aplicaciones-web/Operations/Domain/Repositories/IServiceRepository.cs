using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.Operations.Domain.Repositories;

public interface IServiceRepository : IBaseRepository<Service>
{
    Task<Service?> FindServiceByIdAsync(ServiceId id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Service>> FindAllByBranchIdAsync(BranchId branchId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByIdAsync(ServiceId id, CancellationToken cancellationToken = default);
}
