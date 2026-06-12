using System;
using atelier_platform_aplicaciones_web.Inventory.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Inventory.Domain.Model.Entities;

public class ProductBatch
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public Guid BranchId { get; private set; }
    public InventoryQuantity Quantity { get; private set; }
    public InventoryQuantity AvailableQuantity { get; private set; }
    public Money AcquisitionCost { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public InventoryQuantity ReservedQuantity => new InventoryQuantity(Quantity.Value - AvailableQuantity.Value);

    protected ProductBatch()
    {
    }

    public ProductBatch(Guid productId, Guid branchId, int quantity, Money acquisitionCost)
    {
        Id = Guid.NewGuid();
        ProductId = productId;
        BranchId = branchId;
        Quantity = new InventoryQuantity(quantity);
        AvailableQuantity = new InventoryQuantity(quantity);
        AcquisitionCost = acquisitionCost;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
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
