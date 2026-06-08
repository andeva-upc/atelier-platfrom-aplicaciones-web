using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Core.Application.QueryServices;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.Core.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.Core.Application.Internal.QueryServices;

public class BranchQueryService(IBranchRepository branchRepository) : IBranchQueryService
{
    public async Task<Branch?> Handle(GetBranchByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await branchRepository.FindBranchByIdAsync(query.Id, cancellationToken);
    }

    public async Task<IEnumerable<Branch>> Handle(GetAllBranchesByWorkshopIdQuery query, CancellationToken cancellationToken = default)
    {
        return await branchRepository.FindAllByWorkshopIdAsync(query.WorkshopId, cancellationToken);
    }
}
