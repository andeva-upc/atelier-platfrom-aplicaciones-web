using System;

namespace atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Resources;

public record GenerateVoucherResource(Guid QuoteId, Guid BranchId, decimal SubtotalAmount, string Type, string Currency);
