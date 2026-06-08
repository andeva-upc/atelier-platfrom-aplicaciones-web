using System;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Core.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Core.Interfaces.REST.Transform;

public static class AssignSubscriptionCommandFromResourceAssembler
{
    public static AssignSubscriptionCommand ToCommandFromResource(Guid branchId, AssignSubscriptionResource resource)
    {
        return new AssignSubscriptionCommand(
            new BranchId(branchId),
            new SubscriptionPlanId(resource.PlanId),
            Enum.Parse<BillingCycle>(resource.BillingCycle, true),
            new CreditCard(
                resource.CardNumber,
                resource.CardHolderName,
                resource.ExpirationDate,
                resource.Cvv
            )
        );
    }
}
