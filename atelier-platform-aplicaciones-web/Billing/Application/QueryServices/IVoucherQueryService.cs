using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.Queries;

namespace atelier_platform_aplicaciones_web.Billing.Application.QueryServices;

public interface IVoucherQueryService
{
    Task<Voucher?> Handle(GetVoucherByIdQuery query, CancellationToken cancellationToken = default);
    Task<System.Collections.Generic.IEnumerable<Voucher>> Handle(GetVouchersByBranchIdQuery query, CancellationToken cancellationToken = default);
}
