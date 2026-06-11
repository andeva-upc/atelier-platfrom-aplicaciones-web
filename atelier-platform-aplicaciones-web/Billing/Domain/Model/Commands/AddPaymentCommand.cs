using System;

namespace atelier_platform_aplicaciones_web.Billing.Domain.Model.Commands;

public record AddPaymentCommand(Guid VoucherId, decimal Amount, string Method);
