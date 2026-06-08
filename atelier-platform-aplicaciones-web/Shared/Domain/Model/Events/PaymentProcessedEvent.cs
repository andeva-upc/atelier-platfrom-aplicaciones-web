using System;

namespace atelier_platform_aplicaciones_web.Shared.Domain.Model.Events;

/// <summary>
///     Event representing the successful processing of a payment for a work order. 
///     This event is published after the payment has been processed and can be used to trigger subsequent actions, 
///     such as updating the work order status or notifying relevant parties.
/// </summary>
/// <param name="WorkOrderId">The unique identifier of the work order for which the payment was processed</param>
public record PaymentProcessedEvent(Guid WorkOrderId) : IEvent;