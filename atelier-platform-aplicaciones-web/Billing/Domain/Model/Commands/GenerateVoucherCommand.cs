using System;

namespace atelier_platform_aplicaciones_web.Billing.Domain.Model.Commands;

public record GenerateVoucherCommand(Guid QuoteId, string Type, string CustomerDocumentType, string CustomerDocumentNumber, string CustomerName);
