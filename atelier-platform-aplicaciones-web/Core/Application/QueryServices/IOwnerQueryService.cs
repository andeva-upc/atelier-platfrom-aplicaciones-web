using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Queries;

namespace atelier_platform_aplicaciones_web.Core.Application.QueryServices;

public interface IOwnerQueryService
{
    Task<Owner?> Handle(GetOwnerByIdQuery query, CancellationToken cancellationToken = default);
}
