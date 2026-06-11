using System;

namespace atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Resources;

public record CreateQuoteResource(
    Guid WorkOrderId,
    Guid BranchId,
    decimal SubtotalAmount,
    decimal DiscountPercentage
);
