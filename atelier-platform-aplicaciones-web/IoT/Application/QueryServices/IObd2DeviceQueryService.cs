using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Queries;

namespace atelier_platform_aplicaciones_web.IoT.Application.QueryServices;

public interface IObd2DeviceQueryService
{
    Task<Obd2Device?> Handle(GetObd2DeviceByIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Obd2Device>> Handle(GetObd2DevicesByBranchIdQuery query, CancellationToken cancellationToken = default);
}
