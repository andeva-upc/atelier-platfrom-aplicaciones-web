using atelier_platform_aplicaciones_web.Billing.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Billing.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace atelier_platform_aplicaciones_web.Billing.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class VoucherRepository : BaseRepository<Voucher>, IVoucherRepository
{
    public VoucherRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Voucher>> FindByBranchIdAsync(System.Guid branchId)
    {
        return await Context.Set<Voucher>().Where(v => v.BranchId == branchId).ToListAsync();
    }

    public async Task<Voucher?> FindByIdWithPaymentsAsync(System.Guid id)
    {
        return await Context.Set<Voucher>().Include(v => v.Payments).FirstOrDefaultAsync(v => v.Id == id);
    }
}
