using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Queries;

namespace atelier_platform_aplicaciones_web.Core.Application.QueryServices;

public interface IWorkshopQueryService
{
    Task<Workshop?> Handle(GetWorkshopByIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Workshop>> Handle(GetAllWorkshopsByOwnerIdQuery query, CancellationToken cancellationToken = default);
}
