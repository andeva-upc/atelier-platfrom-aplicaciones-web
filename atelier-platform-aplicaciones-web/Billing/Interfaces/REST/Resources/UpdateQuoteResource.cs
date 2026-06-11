namespace atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Resources;

public record UpdateQuoteResource(decimal SubtotalAmount, decimal DiscountPercentage);
