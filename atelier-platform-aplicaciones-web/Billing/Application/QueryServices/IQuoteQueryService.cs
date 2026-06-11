using System.Threading.Tasks;
using System.Collections.Generic;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.Queries;

namespace atelier_platform_aplicaciones_web.Billing.Application.QueryServices;

/// <summary>
///     Defines the query operations for the Quote aggregate.
/// </summary>
public interface IQuoteQueryService
{
    /// <summary>
    ///     Retrieves a specific quote by its unique identifier.
    /// </summary>
    /// <param name="query">The query containing the quote ID.</param>
    /// <returns>The found Quote, or null if it does not exist.</returns>
    Task<Quote?> Handle(GetQuoteByIdQuery query);

    /// <summary>
    ///     Retrieves all quotes associated with a specific branch.
    /// </summary>
    /// <param name="query">The query containing the branch ID.</param>
    /// <returns>A collection of matching Quotes.</returns>
    Task<IEnumerable<Quote>> Handle(GetQuotesByBranchIdQuery query);
}
