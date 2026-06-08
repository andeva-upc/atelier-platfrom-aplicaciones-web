using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Core.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace atelier_platform_aplicaciones_web.Core.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class BranchSubscriptionRepository(AppDbContext context) : BaseRepository<BranchSubscription>(context), IBranchSubscriptionRepository
{
    public async Task<BranchSubscription?> FindBranchSubscriptionByIdAsync(BranchSubscriptionId id, CancellationToken cancellationToken)
    {
        return await Context.Set<BranchSubscription>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<BranchSubscription>> FindAllByBranchIdAsync(BranchId branchId, CancellationToken cancellationToken)
    {
        return await Context.Set<BranchSubscription>()
            .Where(e => e.BranchId == branchId)
            .ToListAsync(cancellationToken);
    }

    public async Task<BranchSubscription?> FindActiveByBranchIdAsync(BranchId branchId, CancellationToken cancellationToken)
    {
        return await Context.Set<BranchSubscription>()
            .Where(e => e.BranchId == branchId && e.Status == SubscriptionStatus.Active)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
