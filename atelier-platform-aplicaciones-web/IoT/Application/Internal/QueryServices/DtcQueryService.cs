using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.IoT.Domain.Repositories;
using atelier_platform_aplicaciones_web.IoT.Domain.Services;

namespace atelier_platform_aplicaciones_web.IoT.Application.Internal.QueryServices;

public class DtcQueryService : IDtcQueryService
{
    private readonly IDtcAlertRepository _dtcAlertRepository;

    public DtcQueryService(IDtcAlertRepository dtcAlertRepository)
    {
        _dtcAlertRepository = dtcAlertRepository;
    }

    public async Task<IEnumerable<DtcAlert>> Handle(GetActiveDtcAlertsQuery query)
    {
        return await _dtcAlertRepository.FindActiveByBranchIdAsync(query.BranchId);
    }
}
