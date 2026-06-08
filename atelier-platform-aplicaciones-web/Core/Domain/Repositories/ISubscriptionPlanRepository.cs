using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.Core.Domain.Repositories;

public interface ISubscriptionPlanRepository : IBaseRepository<SubscriptionPlan>
{
    Task<SubscriptionPlan?> FindSubscriptionPlanByIdAsync(SubscriptionPlanId id, CancellationToken cancellationToken);
    Task<SubscriptionPlan?> FindByNameAsync(string name, CancellationToken cancellationToken);
}
