using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.Core.Domain.Repositories;

public interface IWorkshopRepository : IBaseRepository<Workshop>
{
    Task<Workshop?> FindWorkshopByIdAsync(WorkshopId id, CancellationToken cancellationToken);
    Task<IEnumerable<Workshop>> FindAllByOwnerIdAsync(OwnerId ownerId, CancellationToken cancellationToken);
    Task<bool> ExistsByIdAsync(WorkshopId id, CancellationToken cancellationToken);
}
