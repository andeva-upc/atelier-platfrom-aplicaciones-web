using System;
using atelier_platform_aplicaciones_web.Inventory.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Inventory.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.Inventory.Interfaces.REST.Transform;

public static class AddBatchToProductCommandFromResourceAssembler
{
    public static AddBatchToProductCommand ToCommandFromResource(Guid productId, AddBatchToProductResource resource)
    {
        return new AddBatchToProductCommand(productId, resource.Quantity, resource.AcquisitionCost);
    }
}
