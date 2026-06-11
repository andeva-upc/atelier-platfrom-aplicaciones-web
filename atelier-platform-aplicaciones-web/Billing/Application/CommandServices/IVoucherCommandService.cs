using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Shared.Application.Model;

namespace atelier_platform_aplicaciones_web.Billing.Application.CommandServices;

public interface IVoucherCommandService
{
    Task<Result<Voucher>> Handle(GenerateVoucherCommand command, CancellationToken cancellationToken = default);
    Task<Result<atelier_platform_aplicaciones_web.Billing.Domain.Model.Entities.Payment>> Handle(AddPaymentCommand command, CancellationToken cancellationToken = default);
    Task<Result> Handle(RemovePaymentCommand command, CancellationToken cancellationToken = default);
    Task<Result<Voucher>> Handle(ProcessCheckoutCommand command, CancellationToken cancellationToken = default);
}
