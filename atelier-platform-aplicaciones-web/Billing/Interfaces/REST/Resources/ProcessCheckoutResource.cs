using System;

namespace atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Resources;

public record ProcessCheckoutResource(
    Guid QuoteId,
    string Type,
    string? CustomerDocumentType,
    string? CustomerDocumentNumber,
    string? CustomerName,
    string Method
);
