using System;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.Aggregates;

public partial class WorkOrderTaskProduct
{
    public Guid Id { get; private set; }
    public ProductId ProductId { get; private set; }
    public BranchId BranchId { get; private set; }
    public Quantity Quantity { get; private set; }
    public Money UnitPrice { get; private set; }
    public Money TotalAmount { get; private set; }
    
    protected WorkOrderTaskProduct()
    {
        ProductId = null!;
        BranchId = null!;
        Quantity = null!;
        UnitPrice = null!;
        TotalAmount = null!;
    }

    public WorkOrderTaskProduct(ProductId productId, BranchId branchId, Quantity quantity, Money unitPrice)
    {
        Id = Guid.NewGuid();
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