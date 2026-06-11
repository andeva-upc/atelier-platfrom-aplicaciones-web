using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.Queries;

namespace atelier_platform_aplicaciones_web.Billing.Application.QueryServices;

/// <summary>
///     Defines the query operations for the Voucher aggregate.
/// </summary>
public interface IVoucherQueryService
{
    /// <summary>
    ///     Retrieves a specific voucher by its unique identifier.
    /// </summary>
    /// <param name="query">The query containing the voucher ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The found Voucher, or null if it does not exist.</returns>
    Task<Voucher?> Handle(GetVoucherByIdQuery query, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Retrieves all vouchers associated with a specific branch.
    /// </summary>
    /// <param name="query">The query containing the branch ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A collection of matching Vouchers.</returns>
    Task<System.Collections.Generic.IEnumerable<Voucher>> Handle(GetVouchersByBranchIdQuery query, CancellationToken cancellationToken = default);
}
