using System;

namespace atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Resources;

public record QuoteResource(
    Guid Id,
    Guid WorkshopId,
    Guid CustomerId,
    Guid VehicleId,
    string Description,
    string Currency,
    decimal SubtotalAmount,
    decimal DiscountAmount,
    decimal TaxPercentage,
    decimal TotalAmount,
    string Status
);
