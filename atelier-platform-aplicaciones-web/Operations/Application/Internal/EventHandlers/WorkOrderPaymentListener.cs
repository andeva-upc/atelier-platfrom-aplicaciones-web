using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Operations.Application.CommandServices;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Shared.Application.Internal.EventHandlers;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.Events;

namespace atelier_platform_aplicaciones_web.Operations.Application.Internal.EventHandlers;

public class WorkOrderPaymentListener(IWorkOrderCommandService commandService) 
    : IEventHandler<PaymentProcessedEvent>
{
    public async Task Handle(PaymentProcessedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Reacciona al pago y ejecuta el comando de cambio de estado a Paid de forma transaccional
        await commandService.Handle(new MarkWorkOrderAsPaidCommand(domainEvent.WorkOrderId), cancellationToken);
    }
}
