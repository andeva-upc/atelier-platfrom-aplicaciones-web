using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EFC.Configuration;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace atelier_platform_aplicaciones_web.IoT.Infrastructure.Persistence.EFC.Repositories;

public class OBD2DeviceRepository : BaseRepository<OBD2Device>, IOBD2DeviceRepository
{
    public OBD2DeviceRepository(AppDbContext context) : base(context) {}

    public async Task<OBD2Device?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<OBD2Device>().FirstOrDefaultAsync(d => d.Id == id, cancellationToken);
    }

    public async Task<OBD2Device?> FindByMacAddressAsync(string macAddress, CancellationToken cancellationToken = default)
    {
        return await Context.Set<OBD2Device>().FirstOrDefaultAsync(d => d.MacAddress == macAddress, cancellationToken);
    }

    public async Task<IEnumerable<OBD2Device>> FindByBranchIdAsync(Guid branchId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<OBD2Device>().Where(d => d.BranchId == branchId).ToListAsync(cancellationToken);
    }
}
