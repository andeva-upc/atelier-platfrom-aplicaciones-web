using System;

namespace atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Resources;

public record CreateQuoteResource(
    Guid WorkshopId,
    Guid CustomerId,
    Guid VehicleId,
    string Description,
    string Currency,
    decimal SubtotalAmount,
    decimal TaxPercentage
);
