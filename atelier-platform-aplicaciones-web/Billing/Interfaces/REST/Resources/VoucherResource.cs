using System;

namespace atelier_platform_aplicaciones_web.Billing.Interfaces.REST.Resources;

public record VoucherResource(Guid Id, Guid QuoteId, Guid BranchId, int VoucherNumber, decimal SubtotalAmount, decimal TotalAmount, string Type, string Status, string Currency);
