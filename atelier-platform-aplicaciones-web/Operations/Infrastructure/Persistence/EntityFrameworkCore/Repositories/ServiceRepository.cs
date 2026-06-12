using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Operations.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace atelier_platform_aplicaciones_web.Operations.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ServiceRepository(AppDbContext context) : BaseRepository<Service>(context), IServiceRepository
{
    public async Task<Service?> FindServiceByIdAsync(ServiceId id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Service>().FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Service>> FindAllByBranchIdAsync(BranchId branchId, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Service>()
            .Where(s => s.BranchId == branchId)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByIdAsync(ServiceId id, CancellationToken cancellationToken = default)
    {
        return await Context.Set<Service>().AnyAsync(s => s.Id == id, cancellationToken);
    }
}
