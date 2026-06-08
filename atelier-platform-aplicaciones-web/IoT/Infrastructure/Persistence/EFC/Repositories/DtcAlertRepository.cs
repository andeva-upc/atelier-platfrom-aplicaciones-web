using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EFC.Configuration;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EFC.Repositories;
using Microsoft.EntityFrameworkCore;

namespace atelier_platform_aplicaciones_web.IoT.Infrastructure.Persistence.EFC.Repositories;

public class DtcAlertRepository : BaseRepository<DtcAlert>, IDtcAlertRepository
{
    public DtcAlertRepository(AppDbContext context) : base(context) {}

    public async Task<DtcAlert?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<DtcAlert>().FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<DtcAlert>> FindActiveByBranchIdAsync(Guid branchId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<DtcAlert>()
            .Where(a => a.BranchId == branchId)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
