using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.Core.Domain.Repositories;

public interface IBranchRepository : IBaseRepository<Branch>
{
    Task<Branch?> FindBranchByIdAsync(BranchId id, CancellationToken cancellationToken);
    Task<IEnumerable<Branch>> FindAllByWorkshopIdAsync(WorkshopId workshopId, CancellationToken cancellationToken);
    Task<bool> ExistsByIdAsync(BranchId id, CancellationToken cancellationToken);
    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken);
}
