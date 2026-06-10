namespace atelier_platform_aplicaciones_web.Billing.Domain.Model.Commands;

public record CreateQuoteCommand(
    Guid WorkshopId,
    Guid CustomerId,
    Guid VehicleId,
    string Description,
    string Currency,
    decimal SubtotalAmount,
    decimal TaxPercentage
);
