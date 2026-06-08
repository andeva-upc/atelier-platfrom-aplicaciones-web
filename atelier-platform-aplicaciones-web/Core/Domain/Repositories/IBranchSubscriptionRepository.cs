using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.Core.Domain.Repositories;

public interface IBranchSubscriptionRepository : IBaseRepository<BranchSubscription>
{
    Task<BranchSubscription?> FindBranchSubscriptionByIdAsync(BranchSubscriptionId id, CancellationToken cancellationToken);
    Task<IEnumerable<BranchSubscription>> FindAllByBranchIdAsync(BranchId branchId, CancellationToken cancellationToken);
    Task<BranchSubscription?> FindActiveByBranchIdAsync(BranchId branchId, CancellationToken cancellationToken);
}
