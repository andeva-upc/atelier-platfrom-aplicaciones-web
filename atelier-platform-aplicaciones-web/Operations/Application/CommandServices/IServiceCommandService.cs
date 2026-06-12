using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Shared.Application.Model;

namespace atelier_platform_aplicaciones_web.Operations.Application.CommandServices;

public interface IServiceCommandService
{
    Task<Result<Service>> Handle(CreateServiceCommand command, CancellationToken cancellationToken = default);
    Task<Result<Service>> Handle(UpdateServiceCommand command, CancellationToken cancellationToken = default);
    Task<Result> Handle(DeleteServiceCommand command, CancellationToken cancellationToken = default);
}
