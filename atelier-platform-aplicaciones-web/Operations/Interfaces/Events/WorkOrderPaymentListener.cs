using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Operations.Application.Services;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Shared.Domain.Events;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.Events;

namespace atelier_platform_aplicaciones_web.Operations.Interfaces.Events;

public class WorkOrderPaymentListener(IWorkOrderCommandService commandService) 
    : IDomainEventHandler<PaymentProcessedEvent>
{
    public async Task HandleAsync(PaymentProcessedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Reacciona al pago y ejecuta el comando de cambio de estado a Paid de forma transaccional
        await commandService.Handle(new MarkWorkOrderAsPaidCommand(domainEvent.WorkOrderId), cancellationToken);
    }
}