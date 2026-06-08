using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Core.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace atelier_platform_aplicaciones_web.Core.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class WorkshopRepository(AppDbContext context) : BaseRepository<Workshop>(context), IWorkshopRepository
{
    public async Task<Workshop?> FindWorkshopByIdAsync(WorkshopId id, CancellationToken cancellationToken)
    {
        return await Context.Set<Workshop>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Workshop>> FindAllByOwnerIdAsync(OwnerId ownerId, CancellationToken cancellationToken)
    {
        return await Context.Set<Workshop>()
            .Where(e => e.OwnerId == ownerId)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByIdAsync(WorkshopId id, CancellationToken cancellationToken)
    {
        return await Context.Set<Workshop>().AnyAsync(e => e.Id == id, cancellationToken);
    }

    // Explicit Soft Delete implementation
    void IBaseRepository<Workshop>.Remove(Workshop entity)
    {
        entity.DeletedAt = DateTimeOffset.UtcNow;
        Update(entity);
    }
}
