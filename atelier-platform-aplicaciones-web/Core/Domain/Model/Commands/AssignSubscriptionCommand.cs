using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Core.Domain.Model.Commands;

public record AssignSubscriptionCommand(
    BranchId BranchId,
    SubscriptionPlanId PlanId,
    BillingCycle BillingCycle,
    CreditCard CreditCard
);
