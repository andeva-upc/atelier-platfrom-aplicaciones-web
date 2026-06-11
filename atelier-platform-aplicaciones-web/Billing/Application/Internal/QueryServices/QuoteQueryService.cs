using System.Threading.Tasks;
using System.Collections.Generic;
using atelier_platform_aplicaciones_web.Billing.Application.QueryServices;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.Billing.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.Billing.Application.Internal.QueryServices;

public class QuoteQueryService(IQuoteRepository quoteRepository) : IQuoteQueryService
{
    public async Task<Quote?> Handle(GetQuoteByIdQuery query)
    {
        return await quoteRepository.FindByIdAsync(query.Id);
    }

    public async Task<IEnumerable<Quote>> Handle(GetQuotesByBranchIdQuery query)
    {
        return await quoteRepository.FindByBranchIdAsync(query.BranchId);
    }
}
