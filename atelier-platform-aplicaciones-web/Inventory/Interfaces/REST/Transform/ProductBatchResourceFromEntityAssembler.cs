using atelier_platform_aplicaciones_web.Inventory.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.Inventory.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.Inventory.Interfaces.REST.Transform;

public static class ProductBatchResourceFromEntityAssembler
{
    public static ProductBatchResource ToResourceFromEntity(ProductBatch entity)
    {
        return new ProductBatchResource(entity.Id, entity.AvailableQuantity.Value, entity.ReservedQuantity.Value, entity.AcquisitionCost.Amount, entity.CreatedAt);
    }
}
