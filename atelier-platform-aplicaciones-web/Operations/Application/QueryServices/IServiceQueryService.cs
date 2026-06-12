using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Queries;

namespace atelier_platform_aplicaciones_web.Operations.Application.QueryServices;

public interface IServiceQueryService
{
    Task<Service?> Handle(GetServiceByIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Service>> Handle(GetAllServicesByBranchIdQuery query, CancellationToken cancellationToken = default);
}
