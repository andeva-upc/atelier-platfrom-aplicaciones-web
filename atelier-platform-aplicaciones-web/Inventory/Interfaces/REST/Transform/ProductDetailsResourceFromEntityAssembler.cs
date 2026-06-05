using System.Linq;
using atelier_platform_aplicaciones_web.Inventory.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Inventory.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.Inventory.Interfaces.REST.Transform;

public static class ProductDetailsResourceFromEntityAssembler
{
    public static ProductDetailsResource ToResourceFromEntity(Product entity)
    {
        // Por ahora, como no hay lotes, CurrentStock y los detalles son similares a ProductResource
        // Cuando agreguemos ProductBatch, este DTO incluirá la lista de lotes.
        return new ProductDetailsResource(
            entity.Id,
            entity.BranchId.Value,
            entity.Category.Value,
            entity.Name.Name,
            entity.Sku.Value,
            entity.Description,
            entity.CurrentSellingPrice.Amount,
            entity.MinimumStock,
            entity.CurrentStock.Value,
            entity.Batches.Select(ProductBatchResourceFromEntityAssembler.ToResourceFromEntity)
        );
    }
}
