using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using atelier_platform_aplicaciones_web.IAM.Infrastructure.Pipeline.Middleware.Attributes;

using atelier_platform_aplicaciones_web.Operations.Application.CommandServices;
using atelier_platform_aplicaciones_web.Operations.Application.QueryServices;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Operations.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.Operations.Interfaces.REST.Transform;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Operations.Resources;

namespace atelier_platform_aplicaciones_web.Operations.Interfaces.REST;

[ApiController]
[Route("api/v1/work-orders")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Work Orders")]
[Authorize]
public class WorkOrdersController(
    IWorkOrderCommandService workOrderCommandService,
    IWorkOrderQueryService workOrderQueryService,
    IStringLocalizer<OperationsMessages> localizer)
    : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new Work Order")]
    public async Task<ActionResult> CreateWorkOrder([FromBody] CreateWorkOrderResource resource)
    {
        var command = WorkOrderCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await workOrderCommandService.Handle(command);
        
        return ActionResultFromWorkOrderCommandResultAssembler.ToCreatedAtActionResult(
            result, 
            this, 
            localizer, 
            nameof(GetWorkOrderById));
    }

    [HttpPost("{id}/tasks")]
    [SwaggerOperation(Summary = "Add a mechanic task to a Work Order")]
    public async Task<ActionResult> AddTaskToWorkOrder(Guid id, [FromBody] AddTaskResource resource)
    {
        var command = WorkOrderCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await workOrderCommandService.Handle(command);

        return ActionResultFromWorkOrderCommandResultAssembler.ToOkActionResult(result, this, localizer);
    }

    [HttpPut("{id}/tasks/{taskId}")]
    [SwaggerOperation(Summary = "Update mechanic task details")]
    public async Task<ActionResult> UpdateTaskDetails(Guid id, Guid taskId, [FromBody] UpdateWorkOrderTaskDetailsResource resource)
    {
        var command = WorkOrderCommandFromResourceAssembler.ToCommandFromResource(id, taskId, resource);
        var result = await workOrderCommandService.Handle(command);

        return ActionResultFromWorkOrderCommandResultAssembler.ToOkActionResult(result, this, localizer);
    }

    [HttpPost("{id}/tasks/{taskId}/products")]
    [SwaggerOperation(Summary = "Add an inventory product/part to a task")]
    public async Task<ActionResult> AddProductToTask(Guid id, Guid taskId, [FromBody] AddProductResource resource)
    {
        var command = WorkOrderCommandFromResourceAssembler.ToCommandFromResource(id, taskId, resource);
        var result = await workOrderCommandService.Handle(command);

        return ActionResultFromWorkOrderCommandResultAssembler.ToOkActionResult(result, this, localizer);
    }

    [HttpPut("{id}/tasks/{taskId}/products/{productId}")]
    [SwaggerOperation(Summary = "Update a product's quantity in a task")]
    public async Task<ActionResult> UpdateProductQuantity(Guid id, Guid taskId, Guid productId, [FromBody] UpdateProductQuantityInTaskResource resource)
    {
        var command = WorkOrderCommandFromResourceAssembler.ToCommandFromResource(id, taskId, productId, resource);
        var result = await workOrderCommandService.Handle(command);

        return ActionResultFromWorkOrderCommandResultAssembler.ToOkActionResult(result, this, localizer);
    }

    [HttpDelete("{id}/tasks/{taskId}/products/{productId}")]
    [SwaggerOperation(Summary = "Remove a product/part from a task (releases stock reservation)")]
    public async Task<ActionResult> RemoveProductFromTask(Guid id, Guid taskId, Guid productId)
    {
        var command = new RemoveProductFromTaskCommand(id, taskId, new ProductId(productId));
        var result = await workOrderCommandService.Handle(command);

        return ActionResultFromWorkOrderCommandResultAssembler.ToOkActionResult(result, this, localizer);
    }

    [HttpDelete("{id}/tasks/{taskId}")]
    [SwaggerOperation(Summary = "Remove a task from the Work Order (releases all task's stock reservations)")]
    public async Task<ActionResult> RemoveTaskFromWorkOrder(Guid id, Guid taskId)
    {
        var command = new RemoveTaskFromWorkOrderCommand(id, taskId);
        var result = await workOrderCommandService.Handle(command);

        return ActionResultFromWorkOrderCommandResultAssembler.ToOkActionResult(result, this, localizer);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Soft delete a Work Order (releases all active stock reservations)")]
    public async Task<ActionResult> DeleteWorkOrder(Guid id)
    {
        var command = new DeleteWorkOrderCommand(id);
        var result = await workOrderCommandService.Handle(command);

        return ActionResultFromWorkOrderCommandResultAssembler.ToNoContentActionResult(result, this, localizer);
    }

    [HttpPost("{id}/tasks/{taskId}/start")]
    [SwaggerOperation(Summary = "Start executing a task (sets status to DOING and captures startedAt)")]
    public async Task<ActionResult> StartTask(Guid id, Guid taskId)
    {
        var command = new StartTaskCommand(id, taskId);
        var result = await workOrderCommandService.Handle(command);

        return ActionResultFromWorkOrderCommandResultAssembler.ToOkActionResult(result, this, localizer);
    }

    [HttpPost("{id}/tasks/{taskId}/complete")]
    [SwaggerOperation(Summary = "Complete a task (sets status to COMPLETED and captures completedAt)")]
    public async Task<ActionResult> CompleteTask(Guid id, Guid taskId)
    {
        var command = new CompleteTaskCommand(id, taskId);
        var result = await workOrderCommandService.Handle(command);

        return ActionResultFromWorkOrderCommandResultAssembler.ToOkActionResult(result, this, localizer);
    }

    [HttpPost("{id}/tasks/{taskId}/reopen")]
    [SwaggerOperation(Summary = "Reopen a completed task (returns task to DOING, clears completedAt, keeps stock reserved)")]
    public async Task<ActionResult> ReopenTask(Guid id, Guid taskId)
    {
        var command = new ReopenTaskCommand(id, taskId);
        var result = await workOrderCommandService.Handle(command);

        return ActionResultFromWorkOrderCommandResultAssembler.ToOkActionResult(result, this, localizer);
    }

    [HttpGet("{id}")]
    [ActionName(nameof(GetWorkOrderById))]
    [SwaggerOperation(Summary = "Get a Work Order by ID")]
    public async Task<ActionResult> GetWorkOrderById(Guid id)
    {
        var query = new GetWorkOrderByIdQuery(id);
        var workOrder = await workOrderQueryService.Handle(query);
        if (workOrder == null) return NotFound();
        // Buscamos el código de la sucursal y lo inyectamos
        string branchCode = workOrderQueryService.GetBranchCode(workOrder.BranchId.Value);
        return Ok(WorkOrderResourceFromEntityAssembler.ToResourceFromEntity(workOrder, branchCode));
    }
    [HttpGet("branch/{branchId}")]
    [SwaggerOperation(Summary = "Get all Work Orders for a specific branch")]
    public async Task<ActionResult> GetWorkOrdersByBranch(Guid branchId)
    {
        var query = new GetWorkOrdersByBranchIdQuery(new BranchId(branchId));
        var result = await workOrderQueryService.Handle(query);
        
        // Como todos son de la misma sucursal, consultamos una sola vez
        string branchCode = workOrderQueryService.GetBranchCode(branchId);
        var resources = result.Select(wo => WorkOrderResourceFromEntityAssembler.ToResourceFromEntity(wo, branchCode));
        
        return Ok(resources);
    }
    [HttpGet("vehicle/{vehicleId}")]
    [SwaggerOperation(Summary = "Get all Work Orders (service history) for a specific vehicle")]
    public async Task<ActionResult> GetWorkOrdersByVehicle(Guid vehicleId)
    {
        var query = new GetWorkOrdersByVehicleIdQuery(new VehicleId(vehicleId));
        var result = await workOrderQueryService.Handle(query);
        
        // Iteramos porque un vehículo pudo haber ido a diferentes sucursales
        var resources = result.Select(wo => 
        {
            string branchCode = workOrderQueryService.GetBranchCode(wo.BranchId.Value);
            return WorkOrderResourceFromEntityAssembler.ToResourceFromEntity(wo, branchCode);
        });
        
        return Ok(resources);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update Work Order details (diagnostic and mileage)")]
    public async Task<ActionResult> UpdateWorkOrderDetails(Guid id, [FromBody] UpdateWorkOrderDetailsResource resource)
    {
        var command = WorkOrderCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await workOrderCommandService.Handle(command);

        return ActionResultFromWorkOrderCommandResultAssembler.ToOkActionResult(result, this, localizer);
    }
}
