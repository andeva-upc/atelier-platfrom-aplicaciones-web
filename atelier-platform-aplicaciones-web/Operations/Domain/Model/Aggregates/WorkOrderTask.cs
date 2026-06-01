using System;
using System.Collections.Generic;
using System.Linq;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.Aggregates;

public partial class WorkOrderTask
{
    public Guid Id { get; private set; }
    public ServiceId ServiceId { get; private set; }
    public BranchId BranchId { get; private set; }
    public MechanicId AssignedMechanicId { get; private set; }
    public WorkOrderTaskStatus Status { get; private set; }
    public TaskDescription Description { get; private set; }
    public Money Price { get; private set; }
    public DateTimeOffset? StartedAt { get; private set; }
    public DateTimeOffset? CompletedAt { get; private set; }
    
    private readonly List<WorkOrderTaskProduct> _products = new();
    public IReadOnlyCollection<WorkOrderTaskProduct> Products => _products.AsReadOnly();

    protected WorkOrderTask()
    {
        ServiceId = null!;
        BranchId = null!;
        AssignedMechanicId = null!;
        Description = null!;
        Price = null!;
    }

    public WorkOrderTask(ServiceId serviceId, BranchId branchId, MechanicId mechanicId, TaskDescription description, Money laborPrice)
    {
        Id = Guid.NewGuid();
        ServiceId = serviceId;
        BranchId = branchId;
        AssignedMechanicId = mechanicId;
        Description = description;
        Price = laborPrice;
        Status = WorkOrderTaskStatus.Pending;
    }

    public void AddProduct(ProductId productId, Quantity quantity, Money unitPrice)
    {
        if (Status == WorkOrderTaskStatus.Completed)
        {
            throw new InvalidOperationException("operations.error.task.cannotModifyCompletedTask");
        }

        var existingProduct = _products.FirstOrDefault(p => p.ProductId == productId);

        if (existingProduct != null)
        {
            var oldTotalAmount = existingProduct.TotalAmount;
            existingProduct.UpdateQuantity(new Quantity(existingProduct.Quantity.Value + quantity.Value));
            Price = Price.Minus(oldTotalAmount).Plus(existingProduct.TotalAmount);
        }
        else
        {
            var product = new WorkOrderTaskProduct(productId, BranchId, quantity, unitPrice);
            _products.Add(product);
            Price = Price.Plus(product.TotalAmount);
        }
    }

    public void RemoveProduct(ProductId productId)
    {
        if (Status == WorkOrderTaskStatus.Completed)
        {
            throw new InvalidOperationException("operations.error.task.cannotModifyCompletedTask");
        }

        var product = _products.FirstOrDefault(p => p.ProductId == productId) 
            ?? throw new ArgumentException("operations.error.taskProduct.notFound");

        Price = Price.Minus(product.TotalAmount);
        _products.Remove(product);
    }

    public void Start()
    {
        Status = Status.TransitionTo(WorkOrderTaskStatus.Doing);
        StartedAt = DateTimeOffset.UtcNow;
    }

    public bool Complete()
    {
        Status = Status.TransitionTo(WorkOrderTaskStatus.Completed);
        CompletedAt = DateTimeOffset.UtcNow;
        return true;
    }

    public bool Reopen()
    {
        Status = Status.TransitionTo(WorkOrderTaskStatus.Doing);
        CompletedAt = null;
        return true;
    }

    public void UpdateDetails(ServiceId serviceId, MechanicId mechanicId, TaskDescription description, Money newLaborPrice)
    {
        if (Status == WorkOrderTaskStatus.Completed)
        {
            throw new InvalidOperationException("operations.error.task.cannotModifyCompletedTask");
        }

        ServiceId = serviceId;
        AssignedMechanicId = mechanicId;
        Description = description;

        var productsTotal = _products.Aggregate(Money.Zero, (sum, p) => sum.Plus(p.TotalAmount));
        Price = newLaborPrice.Plus(productsTotal);
    }

    public Quantity UpdateProductQuantity(ProductId productId, Quantity newQuantity)
    {
        if (Status == WorkOrderTaskStatus.Completed)
        {
            throw new InvalidOperationException("operations.error.task.cannotModifyCompletedTask");
        }

        var product = _products.FirstOrDefault(p => p.ProductId == productId) 
            ?? throw new ArgumentException("operations.error.taskProduct.notFound");

        var oldQuantity = product.Quantity;
        var oldTotalAmount = product.TotalAmount;

        product.UpdateQuantity(newQuantity);

        Price = Price.Minus(oldTotalAmount).Plus(product.TotalAmount);

        return oldQuantity;
    }
    
    public bool IsDeleted()
    {
        return DeletedAt != null;
    }
}