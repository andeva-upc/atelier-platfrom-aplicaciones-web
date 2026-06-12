using System;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Shared.Domain.Model;
using atelier_platform_aplicaciones_web.Operations.Domain.Model;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Operations.Application.CommandServices;
using atelier_platform_aplicaciones_web.Operations.Application.QueryServices;
using atelier_platform_aplicaciones_web.Operations.Resources;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Operations.Domain.Repositories;
using atelier_platform_aplicaciones_web.Inventory.Application.QueryServices;
using atelier_platform_aplicaciones_web.Inventory.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.Shared.Application.Model;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;
using Microsoft.Extensions.Localization;

namespace atelier_platform_aplicaciones_web.Operations.Application.Internal.CommandServices;

public class WorkOrderCommandService(
    IWorkOrderRepository workOrderRepository,
    IServiceRepository serviceRepository,
    IProductQueryService productQueryService,
    IUnitOfWork unitOfWork,
    IStringLocalizer<OperationsMessages> localizer) : IWorkOrderCommandService
{
    public async Task<Result<WorkOrder>> Handle(CreateWorkOrderCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            if (await workOrderRepository.ExistsByAppointmentIdAsync(command.AppointmentId, cancellationToken))
            {
                return Result<WorkOrder>.Failure(WorkOrderError.Duplicate, localizer["operations.error.appointmentId.duplicate"]);
            }

            int maxInternalNumber = await workOrderRepository.FindMaxInternalNumberByBranchIdAsync(command.BranchId, cancellationToken);
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

            await workOrderRepository.AddAsync(workOrder, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);

            return Result<WorkOrder>.Success(workOrder);
        }
        catch (Exception)
        {
            return Result<WorkOrder>.Failure(WorkOrderError.UnexpectedError, localizer["operations.error.unexpected"]);
        }
    }

    public async Task<Result<WorkOrder>> Handle(AddTaskToWorkOrderCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var service = await serviceRepository.FindServiceByIdAsync(command.ServiceId, cancellationToken);
            if (service == null)
            {
                return Result<WorkOrder>.Failure(WorkOrderError.NotFound, localizer["operations.error.service.notFound"]);
            }

            return await ExecuteOnAggregateAsync(command.WorkOrderId, wo =>
            {
                wo.AddTask(command.ServiceId, command.MechanicId, command.Description, service.Price);
            }, cancellationToken);
        }
        catch (ArgumentException e)
        {
            return Result<WorkOrder>.Failure(WorkOrderError.NotFound, localizer[e.Message]);
        }
        catch (InvalidOperationException e)
        {
            return Result<WorkOrder>.Failure(WorkOrderError.InvalidState, localizer[e.Message]);
        }
        catch (Exception)
        {
            return Result<WorkOrder>.Failure(WorkOrderError.UnexpectedError, localizer["operations.error.unexpected"]);
        }
    }

    public async Task<Result<WorkOrder>> Handle(AddProductToTaskCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var product = await productQueryService.Handle(new GetProductByIdQuery(command.ProductId.Value), cancellationToken);
            if (product == null)
            {
                return Result<WorkOrder>.Failure(WorkOrderError.NotFound, localizer["operations.error.product.notFound"]);
            }

            return await ExecuteOnAggregateAsync(command.WorkOrderId, wo =>
            {
                wo.AddProductToTask(command.TaskId, command.ProductId, command.Quantity, product.CurrentSellingPrice);
            }, cancellationToken);
        }
        catch (ArgumentException e)
        {
            return Result<WorkOrder>.Failure(WorkOrderError.NotFound, localizer[e.Message]);
        }
        catch (InvalidOperationException e)
        {
            return Result<WorkOrder>.Failure(WorkOrderError.InvalidState, localizer[e.Message]);
        }
        catch (Exception)
        {
            return Result<WorkOrder>.Failure(WorkOrderError.UnexpectedError, localizer["operations.error.unexpected"]);
        }
    }

    public async Task<Result<WorkOrder>> Handle(UpdateProductQuantityInTaskCommand command, CancellationToken cancellationToken = default)
    {
        return await ExecuteOnAggregateAsync(command.WorkOrderId, wo =>
        {
            wo.UpdateProductQuantityInTask(command.TaskId, command.ProductId, command.NewQuantity);
        }, cancellationToken);
    }

    public async Task<Result<WorkOrder>> Handle(RemoveProductFromTaskCommand command, CancellationToken cancellationToken = default)
    {
        return await ExecuteOnAggregateAsync(command.WorkOrderId, wo =>
        {
            wo.RemoveProductFromTask(command.TaskId, command.ProductId);
        }, cancellationToken);
    }

    public async Task<Result<WorkOrder>> Handle(UpdateWorkOrderTaskDetailsCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var service = await serviceRepository.FindServiceByIdAsync(command.ServiceId, cancellationToken);
            if (service == null)
            {
                return Result<WorkOrder>.Failure(WorkOrderError.NotFound, localizer["operations.error.service.notFound"]);
            }

            return await ExecuteOnAggregateAsync(command.WorkOrderId, wo =>
            {
                wo.UpdateTaskDetails(command.TaskId, command.ServiceId, command.MechanicId, command.Description, service.Price);
            }, cancellationToken);
        }
        catch (ArgumentException e)
        {
            return Result<WorkOrder>.Failure(WorkOrderError.NotFound, localizer[e.Message]);
        }
        catch (InvalidOperationException e)
        {
            return Result<WorkOrder>.Failure(WorkOrderError.InvalidState, localizer[e.Message]);
        }
        catch (Exception)
        {
            return Result<WorkOrder>.Failure(WorkOrderError.UnexpectedError, localizer["operations.error.unexpected"]);
        }
    }

    public async Task<Result<WorkOrder>> Handle(RemoveTaskFromWorkOrderCommand command, CancellationToken cancellationToken = default)
    {
        return await ExecuteOnAggregateAsync(command.WorkOrderId, wo =>
        {
            wo.RemoveTask(command.TaskId);
        }, cancellationToken);
    }

    public async Task<Result<WorkOrder>> Handle(StartTaskCommand command, CancellationToken cancellationToken = default)
    {
        return await ExecuteOnAggregateAsync(command.WorkOrderId, wo =>
        {
            wo.StartTask(command.TaskId);
        }, cancellationToken);
    }

    public async Task<Result<WorkOrder>> Handle(CompleteTaskCommand command, CancellationToken cancellationToken = default)
    {
        return await ExecuteOnAggregateAsync(command.WorkOrderId, wo =>
        {
            wo.CompleteTask(command.TaskId);
        }, cancellationToken);
    }

    public async Task<Result<WorkOrder>> Handle(ReopenTaskCommand command, CancellationToken cancellationToken = default)
    {
        return await ExecuteOnAggregateAsync(command.WorkOrderId, wo =>
        {
            wo.ReopenTask(command.TaskId);
        }, cancellationToken);
    }

    public async Task<Result<WorkOrder>> Handle(UpdateWorkOrderDetailsCommand command, CancellationToken cancellationToken = default)
    {
        return await ExecuteOnAggregateAsync(command.WorkOrderId, wo =>
        {
            wo.UpdateDetails(command.DiagnosticSummary, command.MileageIn);
        }, cancellationToken);
    }

    public async Task<Result<WorkOrder>> Handle(MarkWorkOrderAsPaidCommand command, CancellationToken cancellationToken = default)
    {
        return await ExecuteOnAggregateAsync(command.WorkOrderId, wo =>
        {
            wo.MarkAsPaid();
        }, cancellationToken);
    }

    public async Task<Result<WorkOrder>> Handle(DeleteWorkOrderCommand command, CancellationToken cancellationToken = default)
    {
        return await ExecuteOnAggregateAsync(command.WorkOrderId, wo =>
        {
            wo.Delete();
        }, cancellationToken);
    }

    private async Task<Result<WorkOrder>> ExecuteOnAggregateAsync(
        WorkOrderId workOrderId, 
        Action<WorkOrder> action, 
        CancellationToken cancellationToken)
    {
        try
        {
            var workOrder = await workOrderRepository.FindByIdWithTasksAndProductsAsync(workOrderId, cancellationToken);
            if (workOrder == null)
            {
                return Result<WorkOrder>.Failure(WorkOrderError.NotFound, localizer["operations.error.workOrder.notFound"]);
            }

            action(workOrder);

            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<WorkOrder>.Success(workOrder);
        }
        catch (ArgumentException e)
        {
            return Result<WorkOrder>.Failure(WorkOrderError.NotFound, localizer[e.Message]);
        }
        catch (InvalidOperationException e)
        {
            return Result<WorkOrder>.Failure(WorkOrderError.InvalidState, localizer[e.Message]);
        }
        catch (Exception)
        {
            return Result<WorkOrder>.Failure(WorkOrderError.UnexpectedError, localizer["operations.error.unexpected"]);
        }
    }
}
