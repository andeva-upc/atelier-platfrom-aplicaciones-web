using System;

namespace atelier_platform_aplicaciones_web.Shared.Domain.Model.Events;

public record PaymentProcessedEvent(Guid WorkOrderId);