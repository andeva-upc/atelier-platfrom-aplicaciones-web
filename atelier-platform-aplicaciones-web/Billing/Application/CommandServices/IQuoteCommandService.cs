using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Shared.Application.Model;

namespace atelier_platform_aplicaciones_web.Billing.Application.CommandServices;

public interface IQuoteCommandService
{
    Task<Result<Quote>> Handle(CreateQuoteCommand command, CancellationToken cancellationToken = default);
    Task<Result<Quote>> Handle(UpdateQuoteCommand command, CancellationToken cancellationToken = default);
    Task<Result<Quote>> Handle(ApproveQuoteCommand command, CancellationToken cancellationToken = default);
}
