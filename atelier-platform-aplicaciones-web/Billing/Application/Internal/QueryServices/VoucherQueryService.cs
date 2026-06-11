using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Billing.Application.QueryServices;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.Billing.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.Billing.Application.Internal.QueryServices;

public class VoucherQueryService : IVoucherQueryService
{
    private readonly IVoucherRepository _voucherRepository;

    public VoucherQueryService(IVoucherRepository voucherRepository)
    {
        _voucherRepository = voucherRepository;
    }

    public async Task<Voucher?> Handle(GetVoucherByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _voucherRepository.FindByIdAsync(query.Id);
    }

    public async Task<System.Collections.Generic.IEnumerable<Voucher>> Handle(GetVouchersByBranchIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _voucherRepository.FindByBranchIdAsync(query.BranchId);
    }
}
