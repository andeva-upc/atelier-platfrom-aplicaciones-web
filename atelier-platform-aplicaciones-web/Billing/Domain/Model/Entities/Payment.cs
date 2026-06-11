using System;

namespace atelier_platform_aplicaciones_web.Billing.Domain.Model.Entities;

/// <summary>
///     Represents a single payment applied to a specific voucher.
///     Acts as an Entity within the Voucher Aggregate.
/// </summary>
public class Payment
{
    /// <summary>
    ///     Unique identifier for the payment.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    ///     Identifier of the voucher this payment belongs to.
    /// </summary>
    public Guid VoucherId { get; private set; }

    /// <summary>
    ///     Identifier of the branch where the payment was received.
    /// </summary>
    public Guid BranchId { get; private set; }

    /// <summary>
    ///     Amount paid.
    /// </summary>
    public decimal Amount { get; private set; }

    /// <summary>
    ///     Payment method used (e.g., CASH, CREDIT_CARD, YAPE).
    /// </summary>
    public string Method { get; private set; }

    /// <summary>
    ///     Currency of the payment (e.g., PEN).
    /// </summary>
    public string Currency { get; private set; }

    /// <summary>
    ///     Timestamp indicating when the payment was successfully processed.
    /// </summary>
    public DateTimeOffset PaidAt { get; private set; }

    /// <summary>
    ///     Empty constructor required by Entity Framework Core.
    /// </summary>
    protected Payment()
    {
        Method = null!;
        Currency = null!;
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Payment"/> entity.
    /// </summary>
    /// <param name="voucherId">The ID of the parent voucher.</param>
    /// <param name="branchId">The branch ID where the payment is taken.</param>
    /// <param name="amount">The monetary amount paid.</param>
    /// <param name="method">The method of payment.</param>
    /// <param name="currency">The currency used.</param>
    public Payment(Guid voucherId, Guid branchId, decimal amount, string method, string currency)
    {
        Id = Guid.NewGuid();
        VoucherId = voucherId;
        BranchId = branchId;
        Amount = amount;
        Method = method;
        Currency = currency;
        PaidAt = DateTimeOffset.UtcNow;
    }
}
