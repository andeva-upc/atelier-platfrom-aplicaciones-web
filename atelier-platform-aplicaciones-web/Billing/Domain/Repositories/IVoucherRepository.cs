using atelier_platform_aplicaciones_web.Billing.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.Billing.Domain.Repositories;

public interface IVoucherRepository : IBaseRepository<Voucher>
{
    Task<IEnumerable<Voucher>> FindByBranchIdAsync(System.Guid branchId);
    Task<Voucher?> FindByIdWithPaymentsAsync(System.Guid id);
}
