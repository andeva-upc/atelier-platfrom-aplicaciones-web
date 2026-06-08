using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Repositories;

public interface IDtcAlertRepository : IBaseRepository<DtcAlert>
{
    Task<DtcAlert?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<DtcAlert>> FindActiveByBranchIdAsync(BranchId branchId, CancellationToken cancellationToken = default);
}
