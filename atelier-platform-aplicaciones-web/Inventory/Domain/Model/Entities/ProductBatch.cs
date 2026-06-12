using System;
using atelier_platform_aplicaciones_web.Inventory.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Inventory.Domain.Model.Entities;

public class ProductBatch
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public InventoryQuantity Quantity { get; private set; }
    public InventoryQuantity AvailableQuantity { get; private set; }
    public atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects.Money AcquisitionCost { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public InventoryQuantity ReservedQuantity => new InventoryQuantity(Quantity.Value - AvailableQuantity.Value);

    protected ProductBatch()
    {
        Quantity = null!;
        AvailableQuantity = null!;
        AcquisitionCost = null!;
    }

    public ProductBatch(Guid productId, int quantity, atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects.Money acquisitionCost)
    {
        Id = Guid.NewGuid();
        ProductId = productId;
        Quantity = new InventoryQuantity(quantity);
        AvailableQuantity = new InventoryQuantity(quantity);
        AcquisitionCost = acquisitionCost;
        CreatedAt = DateTime.UtcNow;
    }

    public void ReserveStock(int quantity)
    {
        if (quantity > AvailableQuantity.Value)
            throw new InvalidOperationException("Not enough stock available in this batch.");

        AvailableQuantity = new InventoryQuantity(AvailableQuantity.Value - quantity);
    }

    public void ReleaseStock(int quantity)
    {
        if (quantity > ReservedQuantity.Value)
            throw new InvalidOperationException("Cannot release more stock than reserved.");

        AvailableQuantity = new InventoryQuantity(AvailableQuantity.Value + quantity);
    }
}
