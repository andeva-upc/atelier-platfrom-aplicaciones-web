using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Shared.Application.Model;

namespace atelier_platform_aplicaciones_web.Core.Application.CommandServices;

public interface IWorkshopCommandService
{
    Task<Result<Workshop>> Handle(CreateWorkshopCommand command, CancellationToken cancellationToken = default);
    Task<Result<Workshop>> Handle(UpdateWorkshopCommand command, CancellationToken cancellationToken = default);
}
