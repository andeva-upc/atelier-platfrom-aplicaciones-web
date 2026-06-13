using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.IoT.Application.QueryServices;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.IoT.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.IoT.Application.Internal.QueryServices;

public class Obd2DeviceQueryService(IObd2DeviceRepository obd2DeviceRepository) : IObd2DeviceQueryService
{
    public async Task<Obd2Device?> Handle(GetObd2DeviceByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await obd2DeviceRepository.FindObd2DeviceByIdAsync(query.Id, cancellationToken);
    }

    public async Task<IEnumerable<Obd2Device>> Handle(GetObd2DevicesByBranchIdQuery query, CancellationToken cancellationToken = default)
    {
        return await obd2DeviceRepository.FindAllByBranchIdAsync(query.BranchId, cancellationToken);
    }
}
