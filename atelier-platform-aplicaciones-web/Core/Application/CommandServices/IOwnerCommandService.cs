using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Shared.Application.Model;

namespace atelier_platform_aplicaciones_web.Core.Application.CommandServices;

public interface IOwnerCommandService
{
    Task<Result<Owner>> Handle(CreateOwnerCommand command, CancellationToken cancellationToken = default);
    Task<Result<Owner>> Handle(UpdateOwnerCommand command, CancellationToken cancellationToken = default);
    Task<Result> Handle(DeleteOwnerCommand command, CancellationToken cancellationToken = default);
}
