using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Shared.Domain.Model;
using atelier_platform_aplicaciones_web.Shared.Application.Model;

namespace atelier_platform_aplicaciones_web.Operations.Application.CommandServices;

public interface IWorkOrderCommandService
{
    Task<Result<WorkOrder, Error>> Handle(CreateWorkOrderCommand command, CancellationToken cancellationToken = default);
    Task<Result<WorkOrder, Error>> Handle(AddTaskToWorkOrderCommand command, CancellationToken cancellationToken = default);
    Task<Result<WorkOrder, Error>> Handle(AddProductToTaskCommand command, CancellationToken cancellationToken = default);
    Task<Result<WorkOrder, Error>> Handle(UpdateProductQuantityInTaskCommand command, CancellationToken cancellationToken = default);
    Task<Result<WorkOrder, Error>> Handle(RemoveProductFromTaskCommand command, CancellationToken cancellationToken = default);
    Task<Result<WorkOrder, Error>> Handle(UpdateWorkOrderTaskDetailsCommand command, CancellationToken cancellationToken = default);
    Task<Result<WorkOrder, Error>> Handle(RemoveTaskFromWorkOrderCommand command, CancellationToken cancellationToken = default);
    Task<Result<WorkOrder, Error>> Handle(StartTaskCommand command, CancellationToken cancellationToken = default);
    Task<Result<WorkOrder, Error>> Handle(CompleteTaskCommand command, CancellationToken cancellationToken = default);
    Task<Result<WorkOrder, Error>> Handle(ReopenTaskCommand command, CancellationToken cancellationToken = default);
    Task<Result<WorkOrder, Error>> Handle(UpdateWorkOrderDetailsCommand command, CancellationToken cancellationToken = default);
    Task<Result<WorkOrder, Error>> Handle(MarkWorkOrderAsPaidCommand command, CancellationToken cancellationToken = default);
    Task<Result<WorkOrder, Error>> Handle(DeleteWorkOrderCommand command, CancellationToken cancellationToken = default);
}
