using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Operations.Application.QueryServices;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.Operations.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.Operations.Application.Internal.QueryServices;

public class ServiceQueryService(IServiceRepository serviceRepository) : IServiceQueryService
{
    public async Task<Service?> Handle(GetServiceByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await serviceRepository.FindServiceByIdAsync(query.Id, cancellationToken);
    }

    public async Task<IEnumerable<Service>> Handle(GetAllServicesByBranchIdQuery query, CancellationToken cancellationToken = default)
    {
        return await serviceRepository.FindAllByBranchIdAsync(query.BranchId, cancellationToken);
    }
}
