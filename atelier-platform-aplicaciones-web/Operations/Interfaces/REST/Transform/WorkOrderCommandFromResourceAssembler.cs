using System;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Operations.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Operations.Interfaces.REST.Transform;

/// <summary>
///     Assembler class to map incoming HTTP resources into domain-level Commands.
/// </summary>
public static class WorkOrderCommandFromResourceAssembler
{
    public static CreateWorkOrderCommand ToCommandFromResource(CreateWorkOrderResource resource)
    {
        return new CreateWorkOrderCommand(
            new AppointmentId(resource.AppointmentId),
            new BranchId(resource.BranchId),
            new VehicleId(resource.VehicleId),
            new CustomerId(resource.CustomerId),
            new DiagnosticSummary(resource.DiagnosticSummary),
            new Mileage(resource.MileageIn));
    }

    public static AddTaskToWorkOrderCommand ToCommandFromResource(Guid workOrderId, AddTaskResource resource)
    {
        return new AddTaskToWorkOrderCommand(
            new WorkOrderId(workOrderId),
            new ServiceId(resource.ServiceId),
            new MechanicId(resource.MechanicId),
            new TaskDescription(resource.Description));
    }

    public static UpdateWorkOrderTaskDetailsCommand ToCommandFromResource(Guid workOrderId, Guid taskId, UpdateWorkOrderTaskDetailsResource resource)
    {
        return new UpdateWorkOrderTaskDetailsCommand(
            new WorkOrderId(workOrderId),
            new WorkOrderTaskId(taskId),
            new ServiceId(resource.ServiceId),
            new MechanicId(resource.MechanicId),
            new TaskDescription(resource.Description));
    }

    public static AddProductToTaskCommand ToCommandFromResource(Guid workOrderId, Guid taskId, AddProductResource resource)
    {
        return new AddProductToTaskCommand(
            new WorkOrderId(workOrderId),
            new WorkOrderTaskId(taskId),
            new ProductId(resource.ProductId),
            new Quantity(resource.Quantity));
    }

    public static UpdateProductQuantityInTaskCommand ToCommandFromResource(Guid workOrderId, Guid taskId, Guid productId, UpdateProductQuantityInTaskResource resource)
    {
        return new UpdateProductQuantityInTaskCommand(
            new WorkOrderId(workOrderId),
            new WorkOrderTaskId(taskId),
            new ProductId(productId),
            new Quantity(resource.Quantity));
    }

    public static UpdateWorkOrderDetailsCommand ToCommandFromResource(Guid workOrderId, UpdateWorkOrderDetailsResource resource)
    {
        return new UpdateWorkOrderDetailsCommand(
            new WorkOrderId(workOrderId),
            new DiagnosticSummary(resource.DiagnosticSummary),
            new Mileage(resource.MileageIn));
    }
}
