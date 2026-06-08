using System;

namespace atelier_platform_aplicaciones_web.Core.Interfaces.REST.Resources;

public record AssignSubscriptionResource(
    Guid PlanId,
    string BillingCycle,
    string CardNumber,
    string CardHolderName,
    string ExpirationDate,
    string Cvv
);
