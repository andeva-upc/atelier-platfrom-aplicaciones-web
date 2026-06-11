using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Shared.Application.Model;

namespace atelier_platform_aplicaciones_web.Billing.Application.CommandServices;

/// <summary>
///     Defines the command operations for the Voucher aggregate.
/// </summary>
public interface IVoucherCommandService
{
    /// <summary>
    ///     Generates a new voucher based on an approved quote and issues it to Facthub.
    /// </summary>
    /// <param name="command">The generate voucher command containing quote ID and customer details.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A Result containing the generated Voucher or an error.</returns>
    Task<Result<Voucher>> Handle(GenerateVoucherCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Registers a new payment for a given voucher.
    /// </summary>
    /// <param name="command">The add payment command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A Result containing the new Payment entity or an error.</returns>
    Task<Result<atelier_platform_aplicaciones_web.Billing.Domain.Model.Entities.Payment>> Handle(AddPaymentCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Removes a mistakenly registered payment from a voucher.
    /// </summary>
    /// <param name="command">The remove payment command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A successful Result or an error if payment/voucher is not found.</returns>
    Task<Result> Handle(RemovePaymentCommand command, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Processes a complete checkout (Generates voucher and registers total payment immediately).
    /// </summary>
    /// <param name="command">The process checkout command.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A Result containing the generated and paid Voucher or an error.</returns>
    Task<Result<Voucher>> Handle(ProcessCheckoutCommand command, CancellationToken cancellationToken = default);
}
