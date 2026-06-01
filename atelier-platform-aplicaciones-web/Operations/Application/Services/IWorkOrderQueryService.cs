using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Queries;

namespace atelier_platform_aplicaciones_web.Operations.Application.Services;

public interface IWorkOrderQueryService
{
    Task<WorkOrder?> Handle(GetWorkOrderByIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<WorkOrder>> Handle(GetWorkOrdersByBranchIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<WorkOrder>> Handle(GetWorkOrdersByVehicleIdQuery query, CancellationToken cancellationToken = default);
    
    string GetBranchCode(Guid branchId);
}