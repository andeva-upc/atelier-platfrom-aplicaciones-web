using System;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Inventory.Domain.Repositories;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Events;
using atelier_platform_aplicaciones_web.Shared.Application.Internal.EventHandlers;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.Inventory.Application.Internal.EventHandlers;

public class InventoryStockListener(IProductRepository productRepository)
    : IEventHandler<ProductReservedEvent>,
      IEventHandler<ProductReservationCanceledEvent>,
      IEventHandler<WorkOrderPaidEvent>
{
    public async Task Handle(ProductReservedEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.FindByIdAsync(domainEvent.ProductId.Value, cancellationToken);
        if (product != null)
        {
            product.ReserveStock(domainEvent.Quantity.Value);
            productRepository.Update(product);
        }
    }

    public async Task Handle(ProductReservationCanceledEvent domainEvent, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.FindByIdAsync(domainEvent.ProductId.Value, cancellationToken);
        if (product != null)
        {
            product.ReleaseStock(domainEvent.Quantity.Value);
            productRepository.Update(product);
        }
    }

    public Task Handle(WorkOrderPaidEvent domainEvent, CancellationToken cancellationToken = default)
    {
        // Al pagar la orden, no es necesario hacer ninguna deducción adicional
        // ya que el trigger de BD se encarga de sincronizar el stock físico final
        return Task.CompletedTask;
    }
}
