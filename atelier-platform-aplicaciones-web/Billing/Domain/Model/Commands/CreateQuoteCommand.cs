namespace atelier_platform_aplicaciones_web.Billing.Domain.Model.Commands;

public record CreateQuoteCommand(
    Guid WorkOrderId,
    Guid BranchId,
    decimal SubtotalAmount,
    decimal DiscountPercentage
);
