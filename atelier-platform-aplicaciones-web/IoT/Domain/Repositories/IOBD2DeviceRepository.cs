using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Repositories;

public interface IOBD2DeviceRepository : IBaseRepository<OBD2Device>
{
    Task<OBD2Device?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<OBD2Device?> FindByMacAddressAsync(string macAddress, CancellationToken cancellationToken = default);
    Task<IEnumerable<OBD2Device>> FindByBranchIdAsync(BranchId branchId, CancellationToken cancellationToken = default);
}
