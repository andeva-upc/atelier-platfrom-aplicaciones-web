using System;

namespace atelier_platform_aplicaciones_web.Core.Interfaces.REST.Resources;

public record BranchSubscriptionResource(
    Guid Id,
    Guid BranchId,
    Guid PlanId,
    string BillingCycle,
    string Status,
    DateTime StartDate,
    DateTime EndDate
);
