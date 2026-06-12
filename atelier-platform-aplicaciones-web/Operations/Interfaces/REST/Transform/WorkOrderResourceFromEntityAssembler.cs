using System.Linq;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.Operations.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.Operations.Interfaces.REST.Transform;

public static class WorkOrderResourceFromEntityAssembler
{
    public static WorkOrderResource ToResourceFromEntity(WorkOrder entity)
    {
        return ToResourceFromEntity(entity, "WO");
    }
    public static WorkOrderResource ToResourceFromEntity(WorkOrder entity, string branchCode)
    {
        var tasks = entity.Tasks.Select(ToTaskResourceFromEntity);
        string formattedNumber = $"{branchCode}-{entity.InternalNumber:D6}";
        
        return new WorkOrderResource(
            entity.Id.Value,
            entity.AppointmentId.Value,
            entity.BranchId.Value,
            entity.VehicleId.Value,
            entity.CustomerId.Value,
            entity.InternalNumber,
            formattedNumber,
            entity.Status.ToString(),
            entity.DiagnosticSummary.Value,
            entity.MileageIn.Value,
            entity.TotalAmount.Amount,
            tasks,
            entity.CreatedAt,
            entity.UpdatedAt
        );
    }

    private static WorkOrderTaskResource ToTaskResourceFromEntity(WorkOrderTask task)
    {
        var products = task.Products.Select(ToProductResourceFromEntity);
        return new WorkOrderTaskResource(
            task.Id.Value,
            task.ServiceId.Value,
            task.BranchId.Value,
            task.AssignedMechanicId.Value,
            task.Status.ToString(),
            task.Description.Value,
            task.Price.Amount,
            task.StartedAt,
            task.CompletedAt,
            products,
            task.CreatedAt
        );
    }

    private static WorkOrderTaskProductResource ToProductResourceFromEntity(WorkOrderTaskProduct product)
    {
        return new WorkOrderTaskProductResource(
            product.Id.Value,
            product.ProductId.Value,
            product.BranchId.Value,
            product.Quantity.Value,
            product.UnitPrice.Amount,
            product.TotalAmount.Amount,
            product.CreatedAt
        );
    }
}