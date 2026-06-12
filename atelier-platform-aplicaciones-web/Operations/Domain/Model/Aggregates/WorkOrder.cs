using System;
using System.Collections.Generic;
using System.Linq;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Events;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.Events;

namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.Aggregates;

public class WorkOrder : IHasDomainEvents, IUserAuditableEntity
{
    public WorkOrderId Id { get; private set; }
    public AppointmentId AppointmentId { get; private set; }
    public BranchId BranchId { get; private set; }
    public VehicleId VehicleId { get; private set; }
    public CustomerId CustomerId { get; private set; }
    public int InternalNumber { get; private set; }
    public WorkOrderStatus Status { get; private set; }
    public DiagnosticSummary DiagnosticSummary { get; private set; }
    public Mileage MileageIn { get; private set; }
    public Money TotalAmount { get; private set; }
    
    // Campos de Auditoría y Concurrencia Integrados
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }
    public long Version { get; set; }
    
    private readonly List<WorkOrderTask> _tasks = new();
    public IReadOnlyCollection<WorkOrderTask> Tasks => _tasks.AsReadOnly();

    private readonly List<IEvent> _domainEvents = new();
    public IReadOnlyCollection<IEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void RegisterEvent(IEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();

    protected WorkOrder()
    {
        Id = null!;
        AppointmentId = null!;
        BranchId = null!;
        VehicleId = null!;
        CustomerId = null!;
        DiagnosticSummary = null!;
        MileageIn = null!;
        TotalAmount = null!;
    }

    public WorkOrder(AppointmentId appointmentId, BranchId branchId, VehicleId vehicleId, CustomerId customerId, int internalNumber, DiagnosticSummary diagnosticSummary, Mileage mileageIn) : this()
    {
        Id = new WorkOrderId(Guid.NewGuid());
        AppointmentId = appointmentId;
        BranchId = branchId;
        VehicleId = vehicleId;
        CustomerId = customerId;
        InternalNumber = internalNumber;
        DiagnosticSummary = diagnosticSummary;
        MileageIn = mileageIn;
        Status = WorkOrderStatus.Pending;
        TotalAmount = Money.Zero;
    }

    public void AddTask(ServiceId serviceId, MechanicId mechanicId, TaskDescription description, Money laborPrice)
    {
        if (Status == WorkOrderStatus.Completed || Status == WorkOrderStatus.Paid)
        {
            throw new InvalidOperationException("operations.error.workOrder.cannotModifyClosedOrder");
        }
        var task = new WorkOrderTask(serviceId, BranchId, mechanicId, description, laborPrice);
        _tasks.Add(task);
        RecalculateTotalAmount();
    }

    public void AddProductToTask(WorkOrderTaskId taskId, ProductId productId, Quantity quantity, Money unitPrice)
    {
        if (Status == WorkOrderStatus.Completed || Status == WorkOrderStatus.Paid)
        {
            throw new InvalidOperationException("operations.error.workOrder.cannotModifyClosedOrder");
        }
        var task = FindTaskOrThrow(taskId);
        task.AddProduct(productId, quantity, unitPrice);
        RecalculateTotalAmount();
        
        RegisterEvent(new ProductReservedEvent(Id, BranchId, productId, quantity));
    }

    public void RemoveProductFromTask(WorkOrderTaskId taskId, ProductId productId)
    {
        if (Status == WorkOrderStatus.Completed || Status == WorkOrderStatus.Paid)
        {
            throw new InvalidOperationException("operations.error.workOrder.cannotModifyClosedOrder");
        }
        var task = FindTaskOrThrow(taskId);
        var product = task.Products.FirstOrDefault(p => p.ProductId == productId)
            ?? throw new ArgumentException("operations.error.taskProduct.notFound");

        var returnedQuantity = product.Quantity;
        task.RemoveProduct(productId);
        RecalculateTotalAmount();

        RegisterEvent(new ProductReservationCanceledEvent(Id, BranchId, product.ProductId, product.Quantity));
    }

    public void RemoveTask(WorkOrderTaskId taskId)
    {
        if (Status == WorkOrderStatus.Completed || Status == WorkOrderStatus.Paid)
        {
            throw new InvalidOperationException("operations.error.workOrder.cannotModifyClosedOrder");
        }
        var task = FindTaskOrThrow(taskId);
        if (task.Status == WorkOrderTaskStatus.Completed)
        {
            throw new InvalidOperationException("operations.error.workOrder.cannotDeleteCompletedTask");
        }

        foreach (var product in task.Products)
        {
            RegisterEvent(new ProductReservationCanceledEvent(Id, BranchId, product.ProductId, product.Quantity));
        }

        _tasks.Remove(task);
        RecalculateTotalAmount();
    }
    
    public void Delete()
    {
        if (Status == WorkOrderStatus.Paid)
        {
            throw new InvalidOperationException("operations.error.workOrder.cannotDeletePaidOrder");
        }
        DeletedAt = DateTimeOffset.UtcNow;
        foreach (var task in _tasks)
        {
            if (!task.IsDeleted())
            {
                foreach (var product in task.Products)
                {
                    if (!product.IsDeleted())
                    {
                        RegisterEvent(new ProductReservationCanceledEvent(Id, BranchId, product.ProductId, product.Quantity));
                    }
                }
            }
        }
    }

    public void StartTask(WorkOrderTaskId taskId)
    {
        if (Status == WorkOrderStatus.Completed || Status == WorkOrderStatus.Paid)
        {
            throw new InvalidOperationException("operations.error.workOrder.cannotModifyClosedOrder");
        }

        var task = FindTaskOrThrow(taskId);
        task.Start();

        CheckAutoCompletion();
    }

    public void CompleteTask(WorkOrderTaskId taskId)
    {
        var task = FindTaskOrThrow(taskId);
        if (task.Complete())
        {
            bool allTasksCompleted = _tasks.All(t => t.Status == WorkOrderTaskStatus.Completed);

            if (allTasksCompleted)
            {
                Status = WorkOrderStatus.Completed;
            }
            else
            {
                CheckAutoCompletion();
            }
        }
    }

    public void ReopenTask(WorkOrderTaskId taskId)
    {
        if (Status == WorkOrderStatus.Paid)
        {
            throw new InvalidOperationException("operations.error.workOrder.cannotReopenTaskOfPaidOrder");
        }
        var task = FindTaskOrThrow(taskId);
        if (task.Reopen())
        {
            if (Status == WorkOrderStatus.Completed)
            {
                Status = WorkOrderStatus.InProgress;
            }
        }
    }

    public void StartWork()
    {
        Status = Status.TransitionTo(WorkOrderStatus.InProgress);
    }

    public void CompleteWorkOrder()
    {
        bool allTasksCompleted = _tasks.All(t => t.Status == WorkOrderTaskStatus.Completed);
        if (!allTasksCompleted)
        {
            throw new InvalidOperationException("operations.error.workOrder.pendingTasksExist");
        }
        Status = Status.TransitionTo(WorkOrderStatus.Completed);
    }

    public void MarkAsPaid()
    {
        Status = Status.TransitionTo(WorkOrderStatus.Paid);

        var dispatchedProducts = _tasks.SelectMany(t => t.Products).ToList();
        RegisterEvent(new WorkOrderPaidEvent(Id, BranchId, dispatchedProducts));
    }

    public void UpdateDetails(DiagnosticSummary diagnosticSummary, Mileage mileageIn)
    {
        if (Status == WorkOrderStatus.Completed || Status == WorkOrderStatus.Paid)
        {
            throw new InvalidOperationException("operations.error.workOrder.cannotModifyClosedOrder");
        }
        DiagnosticSummary = diagnosticSummary;
        MileageIn = mileageIn;
    }

    public void UpdateTaskDetails(WorkOrderTaskId taskId, ServiceId serviceId, MechanicId mechanicId, TaskDescription description, Money laborPrice)
    {
        if (Status == WorkOrderStatus.Completed || Status == WorkOrderStatus.Paid)
        {
            throw new InvalidOperationException("operations.error.workOrder.cannotModifyClosedOrder");
        }
        var task = FindTaskOrThrow(taskId);
        task.UpdateDetails(serviceId, mechanicId, description, laborPrice);
        RecalculateTotalAmount();
    }

    public void UpdateProductQuantityInTask(WorkOrderTaskId taskId, ProductId productId, Quantity newQuantity)
    {
        if (Status == WorkOrderStatus.Completed || Status == WorkOrderStatus.Paid)
        {
            throw new InvalidOperationException("operations.error.workOrder.cannotModifyClosedOrder");
        }
        var task = FindTaskOrThrow(taskId);

        var oldQuantity = task.UpdateProductQuantity(productId, newQuantity);
        RecalculateTotalAmount();

        int delta = newQuantity.Value - oldQuantity.Value;
        if (delta > 0)
        {
            RegisterEvent(new ProductReservedEvent(Id, BranchId, productId, new Quantity(delta)));
        }
        else if (delta < 0)
        {
            RegisterEvent(new ProductReservationCanceledEvent(Id, BranchId, productId, new Quantity(Math.Abs(delta))));
        }
    }

    private void RecalculateTotalAmount()
    {
        TotalAmount = _tasks.Aggregate(Money.Zero, (sum, t) => sum.Plus(t.Price));
    }

    private WorkOrderTask FindTaskOrThrow(WorkOrderTaskId taskId)
    {
        return _tasks.FirstOrDefault(t => t.Id == taskId)
            ?? throw new ArgumentException("operations.error.task.notFound");
    }

    private void CheckAutoCompletion()
    {
        if (Status == WorkOrderStatus.Pending)
        {
            Status = WorkOrderStatus.InProgress;
        }
    }
}