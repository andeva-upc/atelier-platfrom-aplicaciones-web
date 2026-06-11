using System;

namespace atelier_platform_aplicaciones_web.Billing.Domain.Model.Commands;

public record GenerateVoucherCommand(Guid QuoteId, Guid BranchId, decimal SubtotalAmount, string Type, string Currency);
