using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Queries;

namespace atelier_platform_aplicaciones_web.Core.Application.QueryServices;

public interface IProfileQueryService
{
    Task<IEnumerable<string>> Handle(GetProfileRolesByUserIdQuery query, CancellationToken cancellationToken = default);
}
