using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Repositories;

public interface IObd2DeviceRepository : IBaseRepository<Obd2Device>
{
    Task<Obd2Device?> FindObd2DeviceByIdAsync(Obd2DeviceId id, CancellationToken cancellationToken);
    Task<IEnumerable<Obd2Device>> FindAllByBranchIdAsync(BranchId branchId, CancellationToken cancellationToken);
    Task<IEnumerable<Obd2Device>> FindAvailableByBranchIdAsync(BranchId branchId, CancellationToken cancellationToken);
    Task<bool> ExistsByIdAsync(Obd2DeviceId id, CancellationToken cancellationToken);
    Task<bool> ExistsByMacAddressAsync(string macAddress, CancellationToken cancellationToken);
}
