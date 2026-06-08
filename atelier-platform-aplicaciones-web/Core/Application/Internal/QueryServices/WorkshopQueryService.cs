using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Core.Application.QueryServices;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.Core.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.Core.Application.Internal.QueryServices;

public class WorkshopQueryService(IWorkshopRepository workshopRepository) : IWorkshopQueryService
{
    public async Task<Workshop?> Handle(GetWorkshopByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await workshopRepository.FindWorkshopByIdAsync(query.Id, cancellationToken);
    }

    public async Task<IEnumerable<Workshop>> Handle(GetAllWorkshopsByOwnerIdQuery query, CancellationToken cancellationToken = default)
    {
        return await workshopRepository.FindAllByOwnerIdAsync(query.OwnerId, cancellationToken);
    }
}
