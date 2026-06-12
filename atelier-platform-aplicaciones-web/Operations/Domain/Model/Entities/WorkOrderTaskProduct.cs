using System;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.Entities;

namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.Entities;

public class WorkOrderTaskProduct : IAuditableEntity
{
    public WorkOrderTaskProductId Id { get; private set; }
    public ProductId ProductId { get; private set; }
    public BranchId BranchId { get; private set; }
    public Quantity Quantity { get; private set; }
    public Money UnitPrice { get; private set; }
    public Money TotalAmount { get; private set; }
    
    // Campos de Auditoría y Concurrencia Integrados
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public long Version { get; set; }

    protected WorkOrderTaskProduct()
    {
        Id = null!;
        ProductId = null!;
        BranchId = null!;
        Quantity = null!;
        UnitPrice = null!;
        TotalAmount = null!;
    }

    public WorkOrderTaskProduct(ProductId productId, BranchId branchId, Quantity quantity, Money unitPrice) : this()
    {
        Id = new WorkOrderTaskProductId(Guid.NewGuid());
        ProductId = productId;
        BranchId = branchId;
        Quantity = quantity;
        UnitPrice = unitPrice;
        TotalAmount = unitPrice.Multiply(quantity.Value);
    }

    public void UpdateQuantity(Quantity newQuantity)
    {
        Quantity = newQuantity;
        TotalAmount = UnitPrice.Multiply(newQuantity.Value);
    }
    
    public bool IsDeleted()
    {
        return DeletedAt != null;
    }
}
