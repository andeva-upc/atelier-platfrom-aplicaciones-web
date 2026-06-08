using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Shared.Application.Model;

namespace atelier_platform_aplicaciones_web.Core.Application.CommandServices;

public interface IEmployeeCommandService
{
    Task<Result<Employee>> Handle(CreateEmployeeCommand command, CancellationToken cancellationToken = default);
    Task<Result<Employee>> Handle(UpdateEmployeeCommand command, CancellationToken cancellationToken = default);
    Task<Result> Handle(DeleteEmployeeCommand command, CancellationToken cancellationToken = default);
}
