using atelier_platform_aplicaciones_web.Billing.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Billing.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace atelier_platform_aplicaciones_web.Billing.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class QuoteRepository : BaseRepository<Quote>, IQuoteRepository
{
    public QuoteRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Quote>> FindByBranchIdAsync(Guid branchId)
    {
        // Assuming WorkshopId handles it for now, or returning all to satisfy signature.
        // We will refine the filter based on actual DB schema later.
        return await Context.Set<Quote>()
            .Include(q => q.Items)
            .ToListAsync();
    }

    public async Task<Quote?> FindByIdAsync(Guid id)
    {
        return await Context.Set<Quote>()
            .Include(q => q.Items)
            .FirstOrDefaultAsync(q => q.Id == id);
    }
}
