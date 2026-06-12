using System;
using atelier_platform_aplicaciones_web.Inventory.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Inventory.Domain.Model.Aggregates;

public partial class Product : IHasDomainEvents
{
    public Guid Id { get; private set; }
    public BranchId BranchId { get; private set; }
    public ProductCategory Category { get; private set; }
    public ProductName Name { get; private set; }
    public Sku Sku { get; private set; }
    public string Description { get; private set; }
    public Money CurrentSellingPrice { get; private set; }
    public InventoryQuantity CurrentStock { get; private set; }
    public int MinimumStock { get; private set; }

    private readonly System.Collections.Generic.List<atelier_platform_aplicaciones_web.Shared.Domain.Model.Events.IEvent> _domainEvents = new();
    public System.Collections.Generic.IReadOnlyCollection<atelier_platform_aplicaciones_web.Shared.Domain.Model.Events.IEvent> DomainEvents => _domainEvents.AsReadOnly();

    private readonly System.Collections.Generic.List<atelier_platform_aplicaciones_web.Inventory.Domain.Model.Entities.ProductBatch> _batches = new();
    public System.Collections.Generic.IReadOnlyCollection<atelier_platform_aplicaciones_web.Inventory.Domain.Model.Entities.ProductBatch> Batches => _batches.AsReadOnly();

    protected void RegisterEvent(atelier_platform_aplicaciones_web.Shared.Domain.Model.Events.IEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();

    protected Product()
    {
        BranchId = null!;
        Category = null!;
        Name = null!;
        Sku = null!;
        Description = string.Empty;
        CurrentSellingPrice = null!;
        CurrentStock = null!;
    }

    public Product(BranchId branchId, ProductCategory category, ProductName name, Sku sku, string description, Money salePrice, int minimumStock)
    {
        Id = Guid.NewGuid();
        BranchId = branchId;
        Category = category;
        Name = name;
        Sku = sku;
        Description = description;
        CurrentSellingPrice = salePrice;
        MinimumStock = minimumStock;
        CurrentStock = new InventoryQuantity(0);

        RegisterEvent(new atelier_platform_aplicaciones_web.Inventory.Domain.Model.Events.ProductCreatedEvent(Id, BranchId.Value));
    }

    public void UpdateInformation(ProductCategory category, ProductName name, Sku sku, string description, Money salePrice, int minimumStock)
    {
        Category = category;
        Name = name;
        Sku = sku;
        Description = description;
        CurrentSellingPrice = salePrice;
        MinimumStock = minimumStock;
    }

    public void AddBatch(int quantity, atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects.Money acquisitionCost)
    {
        var batch = new atelier_platform_aplicaciones_web.Inventory.Domain.Model.Entities.ProductBatch(Id, BranchId.Value, quantity, acquisitionCost);
        _batches.Add(batch);
        CurrentStock = new InventoryQuantity(CurrentStock.Value + quantity);
    }

    public void ReserveStock(Guid batchId, int quantity)
    {
        var batch = _batches.Find(b => b.Id == batchId);
        if (batch == null)
            throw new InvalidOperationException("Batch not found.");

        batch.ReserveStock(quantity);
        CurrentStock = new InventoryQuantity(CurrentStock.Value - quantity);

        RegisterEvent(new atelier_platform_aplicaciones_web.Inventory.Domain.Model.Events.ProductReservedEvent(Id, quantity, BranchId.Value));
    }

    public void ReserveStock(int quantity)
    {
        if (CurrentStock.Value < quantity)
            throw new InvalidOperationException("Not enough stock available.");

        int remainingToReserve = quantity;
        foreach (var batch in _batches)
        {
            if (remainingToReserve <= 0) break;
            int batchAvail = batch.AvailableQuantity.Value;
            if (batchAvail > 0)
            {
                int deduct = Math.Min(batchAvail, remainingToReserve);
                batch.ReserveStock(deduct);
                remainingToReserve -= deduct;
            }
        }
        CurrentStock = new InventoryQuantity(CurrentStock.Value - quantity);
    }

    public void ReleaseStock(int quantity)
    {
        int remainingToRelease = quantity;
        foreach (var batch in _batches)
        {
            if (remainingToRelease <= 0) break;
            int batchReserved = batch.ReservedQuantity.Value;
            if (batchReserved > 0)
            {
                int release = Math.Min(batchReserved, remainingToRelease);
                batch.ReleaseStock(release);
                remainingToRelease -= release;
            }
        }
        CurrentStock = new InventoryQuantity(CurrentStock.Value + quantity);
    }
}
