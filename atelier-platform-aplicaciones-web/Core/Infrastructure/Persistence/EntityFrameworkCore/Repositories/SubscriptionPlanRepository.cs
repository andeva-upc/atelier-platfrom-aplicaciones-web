using System;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Core.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace atelier_platform_aplicaciones_web.Core.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class SubscriptionPlanRepository(AppDbContext context) : BaseRepository<SubscriptionPlan>(context), ISubscriptionPlanRepository
{
    public async Task<SubscriptionPlan?> FindSubscriptionPlanByIdAsync(SubscriptionPlanId id, CancellationToken cancellationToken)
    {
        return await Context.Set<SubscriptionPlan>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<SubscriptionPlan?> FindByNameAsync(string name, CancellationToken cancellationToken)
    {
        return await Context.Set<SubscriptionPlan>().FirstOrDefaultAsync(e => e.Name == name, cancellationToken);
    }
}
