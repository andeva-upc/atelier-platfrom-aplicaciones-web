using atelier_platform_aplicaciones_web.Core.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.Core.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.Core.Interfaces.REST.Transform;

public static class BranchSubscriptionResourceFromEntityAssembler
{
    public static BranchSubscriptionResource ToResourceFromEntity(BranchSubscription entity)
    {
        return new BranchSubscriptionResource(
            entity.Id?.Value ?? System.Guid.Empty,
            entity.BranchId?.Value ?? System.Guid.Empty,
            entity.PlanId?.Value ?? System.Guid.Empty,
            entity.BillingCycle.ToString(),
            entity.Status.ToString(),
            entity.StartDate,
            entity.EndDate
        );
    }
}
