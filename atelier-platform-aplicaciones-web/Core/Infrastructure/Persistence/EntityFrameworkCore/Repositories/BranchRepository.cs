using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Core.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace atelier_platform_aplicaciones_web.Core.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class BranchRepository(AppDbContext context) : BaseRepository<Branch>(context), IBranchRepository
{
    public async Task<Branch?> FindBranchByIdAsync(BranchId id, CancellationToken cancellationToken)
    {
        return await Context.Set<Branch>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Branch>> FindAllByWorkshopIdAsync(WorkshopId workshopId, CancellationToken cancellationToken)
    {
        return await Context.Set<Branch>()
            .Where(e => e.WorkshopId == workshopId)
            .ToListAsync(cancellationToken);
    }

    public async Task<bool> ExistsByIdAsync(BranchId id, CancellationToken cancellationToken)
    {
        return await Context.Set<Branch>().AnyAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken)
    {
        return await Context.Set<Branch>().AnyAsync(e => e.Code == code, cancellationToken);
    }

    // Explicit Soft Delete implementation
    void IBaseRepository<Branch>.Remove(Branch entity)
    {
        entity.DeletedAt = DateTimeOffset.UtcNow;
        Update(entity);
    }
}
