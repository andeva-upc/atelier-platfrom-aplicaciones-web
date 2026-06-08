using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Shared.Application.Model;

namespace atelier_platform_aplicaciones_web.Core.Application.CommandServices;

public interface IBranchCommandService
{
    Task<Result<Branch>> Handle(CreateBranchCommand command, CancellationToken cancellationToken = default);
    Task<Result<Branch>> Handle(UpdateBranchCommand command, CancellationToken cancellationToken = default);
}
