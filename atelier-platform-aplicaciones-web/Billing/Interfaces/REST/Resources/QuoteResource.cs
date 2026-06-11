using System;

namespace atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Resources;

public record QuoteResource(
    Guid Id,
    Guid WorkOrderId,
    Guid BranchId,
    decimal SubtotalAmount,
    decimal DiscountPercentage,
    decimal TotalAmount,
    string Status
);
