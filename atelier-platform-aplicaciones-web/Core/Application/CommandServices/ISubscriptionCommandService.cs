using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Shared.Application.Model;

namespace atelier_platform_aplicaciones_web.Core.Application.CommandServices;

public interface ISubscriptionCommandService
{
    Task<Result<BranchSubscription>> Handle(AssignSubscriptionCommand command, CancellationToken cancellationToken = default);
    Task<Result<BranchSubscription>> Handle(CancelSubscriptionCommand command, CancellationToken cancellationToken = default);
}
