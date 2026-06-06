using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Shared.Domain.Model;
using atelier_platform_aplicaciones_web.Shared.Application.Model;

namespace atelier_platform_aplicaciones_web.Operations.Application.CommandServices;

public interface IWorkOrderCommandService
{
    Task<Result<WorkOrder>> Handle(CreateWorkOrderCommand command, CancellationToken cancellationToken = default);
    Task<Result<WorkOrder>> Handle(AddTaskToWorkOrderCommand command, CancellationToken cancellationToken = default);
    Task<Result<WorkOrder>> Handle(AddProductToTaskCommand command, CancellationToken cancellationToken = default);
    Task<Result<WorkOrder>> Handle(UpdateProductQuantityInTaskCommand command, CancellationToken cancellationToken = default);
    Task<Result<WorkOrder>> Handle(RemoveProductFromTaskCommand command, CancellationToken cancellationToken = default);
    Task<Result<WorkOrder>> Handle(UpdateWorkOrderTaskDetailsCommand command, CancellationToken cancellationToken = default);
    Task<Result<WorkOrder>> Handle(RemoveTaskFromWorkOrderCommand command, CancellationToken cancellationToken = default);
    Task<Result<WorkOrder>> Handle(StartTaskCommand command, CancellationToken cancellationToken = default);
    Task<Result<WorkOrder>> Handle(CompleteTaskCommand command, CancellationToken cancellationToken = default);
    Task<Result<WorkOrder>> Handle(ReopenTaskCommand command, CancellationToken cancellationToken = default);
    Task<Result<WorkOrder>> Handle(UpdateWorkOrderDetailsCommand command, CancellationToken cancellationToken = default);
    Task<Result<WorkOrder>> Handle(MarkWorkOrderAsPaidCommand command, CancellationToken cancellationToken = default);
    Task<Result<WorkOrder>> Handle(DeleteWorkOrderCommand command, CancellationToken cancellationToken = default);
}
