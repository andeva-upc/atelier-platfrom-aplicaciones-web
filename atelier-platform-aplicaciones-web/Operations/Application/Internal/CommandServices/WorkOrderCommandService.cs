using System;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Shared.Domain.Model;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Operations.Application.CommandServices;
using atelier_platform_aplicaciones_web.Operations.Application.QueryServices;

using atelier_platform_aplicaciones_web.Operations.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Operations.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Application.Model;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.Operations.Application.Internal.CommandServices;

public class WorkOrderCommandService : IWorkOrderCommandService
{
    private readonly IWorkOrderRepository _workOrderRepository;
    private readonly IUnitOfWork _unitOfWork;

    public WorkOrderCommandService(IWorkOrderRepository workOrderRepository, IUnitOfWork unitOfWork)
    {
        _workOrderRepository = workOrderRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<WorkOrder, Error>> Handle(CreateWorkOrderCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            if (await _workOrderRepository.ExistsByAppointmentIdAsync(command.AppointmentId, cancellationToken))
            {
                return new Result<WorkOrder, Error>.Failure(WorkOrderErrors.Duplicate);
            }

            // Calculamos secuencialmente el número interno de la orden en el servidor
            int maxInternalNumber = await _workOrderRepository.FindMaxInternalNumberByBranchIdAsync(command.BranchId, cancellationToken);
            int nextInternalNumber = maxInternalNumber + 1;

            var workOrder = new WorkOrder(
                command.AppointmentId,
                command.BranchId,
                command.VehicleId,
                command.CustomerId,
                nextInternalNumber,
                command.DiagnosticSummary,
                command.MileageIn
            );

            await _workOrderRepository.AddAsync(workOrder, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return new Result<WorkOrder, Error>.Success(workOrder);
        }
        catch (Exception)
        {
            return new Result<WorkOrder, Error>.Failure(WorkOrderErrors.UnexpectedError);
        }
    }

    public async Task<Result<WorkOrder, Error>> Handle(AddTaskToWorkOrderCommand command, CancellationToken cancellationToken = default)
    {
        return await ExecuteOnAggregateAsync(command.WorkOrderId, wo =>
        {
            wo.AddTask(command.ServiceId, command.MechanicId, command.Description, command.LaborPrice);
        }, cancellationToken);
    }

    public async Task<Result<WorkOrder, Error>> Handle(AddProductToTaskCommand command, CancellationToken cancellationToken = default)
    {
        return await ExecuteOnAggregateAsync(command.WorkOrderId, wo =>
        {
            wo.AddProductToTask(command.TaskId, command.ProductId, command.Quantity, command.UnitPrice);
        }, cancellationToken);
    }

    public async Task<Result<WorkOrder, Error>> Handle(UpdateProductQuantityInTaskCommand command, CancellationToken cancellationToken = default)
    {
        return await ExecuteOnAggregateAsync(command.WorkOrderId, wo =>
        {
            wo.UpdateProductQuantityInTask(command.TaskId, command.ProductId, command.NewQuantity);
        }, cancellationToken);
    }

    public async Task<Result<WorkOrder, Error>> Handle(RemoveProductFromTaskCommand command, CancellationToken cancellationToken = default)
    {
        return await ExecuteOnAggregateAsync(command.WorkOrderId, wo =>
        {
            wo.RemoveProductFromTask(command.TaskId, command.ProductId);
        }, cancellationToken);
    }

    public async Task<Result<WorkOrder, Error>> Handle(UpdateWorkOrderTaskDetailsCommand command, CancellationToken cancellationToken = default)
    {
        return await ExecuteOnAggregateAsync(command.WorkOrderId, wo =>
        {
            wo.UpdateTaskDetails(command.TaskId, command.ServiceId, command.MechanicId, command.Description, command.LaborPrice);
        }, cancellationToken);
    }

    public async Task<Result<WorkOrder, Error>> Handle(RemoveTaskFromWorkOrderCommand command, CancellationToken cancellationToken = default)
    {
        return await ExecuteOnAggregateAsync(command.WorkOrderId, wo =>
        {
            wo.RemoveTask(command.TaskId);
        }, cancellationToken);
    }

    public async Task<Result<WorkOrder, Error>> Handle(StartTaskCommand command, CancellationToken cancellationToken = default)
    {
        return await ExecuteOnAggregateAsync(command.WorkOrderId, wo =>
        {
            wo.StartTask(command.TaskId);
        }, cancellationToken);
    }

    public async Task<Result<WorkOrder, Error>> Handle(CompleteTaskCommand command, CancellationToken cancellationToken = default)
    {
        return await ExecuteOnAggregateAsync(command.WorkOrderId, wo =>
        {
            wo.CompleteTask(command.TaskId);
        }, cancellationToken);
    }

    public async Task<Result<WorkOrder, Error>> Handle(ReopenTaskCommand command, CancellationToken cancellationToken = default)
    {
        return await ExecuteOnAggregateAsync(command.WorkOrderId, wo =>
        {
            wo.ReopenTask(command.TaskId);
        }, cancellationToken);
    }

    public async Task<Result<WorkOrder, Error>> Handle(UpdateWorkOrderDetailsCommand command, CancellationToken cancellationToken = default)
    {
        return await ExecuteOnAggregateAsync(command.WorkOrderId, wo =>
        {
            wo.UpdateDetails(command.DiagnosticSummary, command.MileageIn);
        }, cancellationToken);
    }

    public async Task<Result<WorkOrder, Error>> Handle(MarkWorkOrderAsPaidCommand command, CancellationToken cancellationToken = default)
    {
        return await ExecuteOnAggregateAsync(command.WorkOrderId, wo =>
        {
            wo.MarkAsPaid();
        }, cancellationToken);
    }

    public async Task<Result<WorkOrder, Error>> Handle(DeleteWorkOrderCommand command, CancellationToken cancellationToken = default)
    {
        return await ExecuteOnAggregateAsync(command.WorkOrderId, wo =>
        {
            wo.Delete();
        }, cancellationToken);
    }

    // Helper genérico para encapsular el comportamiento común:
    // 1. Obtener la raíz del agregado con carga ansiosa de sus hijos
    // 2. Ejecutar la acción lógica sobre el agregado
    // 3. Persistir y guardar transaccionalmente
    private async Task<Result<WorkOrder, Error>> ExecuteOnAggregateAsync(
        Guid workOrderId, 
        Action<WorkOrder> action, 
        CancellationToken cancellationToken)
    {
        try
        {
            var workOrder = await _workOrderRepository.FindByIdWithTasksAndProductsAsync(workOrderId, cancellationToken);
            if (workOrder == null)
            {
                return new Result<WorkOrder, Error>.Failure(WorkOrderErrors.NotFound);
            }

            action(workOrder);

            await _unitOfWork.CompleteAsync(cancellationToken);
            return new Result<WorkOrder, Error>.Success(workOrder);
        }
        catch (ArgumentException e)
        {
            // Pasa la llave específica de no encontrado (ej. "operations.error.task.notFound")
            return new Result<WorkOrder, Error>.Failure(WorkOrderErrors.GenericNotFound(e.Message));
        }
        catch (InvalidOperationException e)
        {
            // Pasa la llave específica de estado inválido (ej. "operations.error.workOrder.cannotDeletePaidOrder")
            return new Result<WorkOrder, Error>.Failure(WorkOrderErrors.InvalidState(e.Message));
        }
        catch (Exception)
        {
            return new Result<WorkOrder, Error>.Failure(WorkOrderErrors.UnexpectedError);
        }
    }
}


