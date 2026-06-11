using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Shared.Application.Model;

namespace atelier_platform_aplicaciones_web.Billing.Application.CommandServices;

/// <summary>
///     Defines the command operations for the Quote aggregate.
/// </summary>
public interface IQuoteCommandService
{
    /// <summary>
    ///     Creates a new draft Quote for a specific Work Order.
    /// </summary>
    /// <param name="command">The create quote command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A Result containing the newly created Quote or an error.</returns>
    Task<Result<Quote>> Handle(CreateQuoteCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Updates the subtotal and discount of an existing draft Quote.
    /// </summary>
    /// <param name="command">The update quote command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A Result containing the updated Quote or an error.</returns>
    Task<Result<Quote>> Handle(UpdateQuoteCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Approves a Quote, allowing it to be billed.
    /// </summary>
    /// <param name="command">The approve quote command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A Result containing the approved Quote or an error.</returns>
    Task<Result<Quote>> Handle(ApproveQuoteCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Cancels a Quote.
    /// </summary>
    /// <param name="command">The cancel quote command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A Result containing the cancelled Quote or an error.</returns>
    Task<Result<Quote>> Handle(CancelQuoteCommand command, CancellationToken cancellationToken = default);
}
