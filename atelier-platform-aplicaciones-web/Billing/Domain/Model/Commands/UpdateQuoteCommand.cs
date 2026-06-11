using System;

namespace atelier_platform_aplicaciones_web.Billing.Domain.Model.Commands;

public record UpdateQuoteCommand(Guid Id, decimal SubtotalAmount, decimal DiscountPercentage);
