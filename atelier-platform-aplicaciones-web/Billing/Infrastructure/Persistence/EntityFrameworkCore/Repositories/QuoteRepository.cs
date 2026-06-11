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
        return await Context.Set<Quote>()
            .ToListAsync();
    }

    public async Task<Quote?> FindByIdAsync(Guid id)
    {
        return await Context.Set<Quote>()
            .FirstOrDefaultAsync(q => q.Id == id);
    }
}
