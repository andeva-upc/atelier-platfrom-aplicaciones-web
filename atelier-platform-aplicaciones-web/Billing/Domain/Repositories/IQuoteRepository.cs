using atelier_platform_aplicaciones_web.Billing.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.Billing.Domain.Repositories;

public interface IQuoteRepository : IBaseRepository<Quote>
{
    // The query for branchId will require joining with WorkOrder or similar,
    // or adding BranchId to the Quote entity if it corresponds directly to a branch.
    // Based on diagram, Quote belongs to WorkshopId, not explicitly BranchId.
    // For the endpoint quotes/branch/{branchId}, we might need to filter.
    Task<IEnumerable<Quote>> FindByBranchIdAsync(Guid branchId);
}
