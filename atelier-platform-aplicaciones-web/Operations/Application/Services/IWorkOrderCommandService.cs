using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Operations.Application.Errors;
using atelier_platform_aplicaciones_web.Shared.Application.Patterns;

namespace atelier_platform_aplicaciones_web.Operations.Application.Services;

public interface IWorkOrderCommandService
{
    Task<Result<WorkOrder, WorkOrderError>> Handle(CreateWorkOrderCommand command, CancellationToken cancellationToken = default);
    Task<Result<WorkOrder, WorkOrderError>> Handle(AddTaskToWorkOrderCommand command, CancellationToken cancellationToken = default);
    Task<Result<WorkOrder, WorkOrderError>> Handle(AddProductToTaskCommand command, CancellationToken cancellationToken = default);
    Task<Result<WorkOrder, WorkOrderError>> Handle(UpdateProductQuantityInTaskCommand command, CancellationToken cancellationToken = default);
    Task<Result<WorkOrder, WorkOrderError>> Handle(RemoveProductFromTaskCommand command, CancellationToken cancellationToken = default);
    Task<Result<WorkOrder, WorkOrderError>> Handle(UpdateWorkOrderTaskDetailsCommand command, CancellationToken cancellationToken = default);
    Task<Result<WorkOrder, WorkOrderError>> Handle(RemoveTaskFromWorkOrderCommand command, CancellationToken cancellationToken = default);
    Task<Result<WorkOrder, WorkOrderError>> Handle(StartTaskCommand command, CancellationToken cancellationToken = default);
    Task<Result<WorkOrder, WorkOrderError>> Handle(CompleteTaskCommand command, CancellationToken cancellationToken = default);
    Task<Result<WorkOrder, WorkOrderError>> Handle(ReopenTaskCommand command, CancellationToken cancellationToken = default);
    Task<Result<WorkOrder, WorkOrderError>> Handle(UpdateWorkOrderDetailsCommand command, CancellationToken cancellationToken = default);
    Task<Result<WorkOrder, WorkOrderError>> Handle(MarkWorkOrderAsPaidCommand command, CancellationToken cancellationToken = default);
    Task<Result<WorkOrder, WorkOrderError>> Handle(DeleteWorkOrderCommand command, CancellationToken cancellationToken = default);
}