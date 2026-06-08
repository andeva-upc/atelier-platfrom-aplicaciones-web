using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Queries;

namespace atelier_platform_aplicaciones_web.Core.Application.QueryServices;

public interface IBranchQueryService
{
    Task<Branch?> Handle(GetBranchByIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Branch>> Handle(GetAllBranchesByWorkshopIdQuery query, CancellationToken cancellationToken = default);
}
