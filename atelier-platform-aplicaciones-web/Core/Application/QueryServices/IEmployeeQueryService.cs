using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Queries;

namespace atelier_platform_aplicaciones_web.Core.Application.QueryServices;

public interface IEmployeeQueryService
{
    Task<Employee?> Handle(GetEmployeeByIdQuery query, CancellationToken cancellationToken = default);
}
