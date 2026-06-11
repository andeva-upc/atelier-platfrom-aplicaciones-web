using atelier_platform_aplicaciones_web.Billing.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.Entities;

namespace atelier_platform_aplicaciones_web.Billing.Domain.Model.Aggregates;

/// <summary>
///     Represents a Quote (Cotización) created for a given Work Order.
///     Acts as an Aggregate Root in the Billing context.
/// </summary>
public class Quote : IUserAuditableEntity
{
    /// <summary>
    ///     Unique identifier for the quote.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    ///     Identifier of the work order this quote belongs to.
    /// </summary>
    public Guid WorkOrderId { get; private set; }

    /// <summary>
    ///     Identifier of the branch where the quote was created.
    /// </summary>
    public Guid BranchId { get; private set; }

    /// <summary>
    ///     Subtotal amount before discounts.
    /// </summary>
    public decimal SubtotalAmount { get; private set; }

    /// <summary>
    ///     Discount percentage applied to the subtotal.
    /// </summary>
    public decimal DiscountPercentage { get; private set; }

    /// <summary>
    ///     Total amount calculated after applying the discount.
    /// </summary>
    public decimal TotalAmount { get; private set; }

    /// <summary>
    ///     Current status of the quote (e.g., DRAFT, APPROVED, CANCELLED).
    /// </summary>
    public QuoteStatus Status { get; private set; }

    // IAuditableEntity properties
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    // IUserAuditableEntity properties
    public Guid? CreatedBy { get; set; }
    public Guid? UpdatedBy { get; set; }

    // Additional fields needed by the schema
    public DateTimeOffset? DeletedAt { get; set; }
    public long Version { get; set; }

    /// <summary>
    ///     Empty constructor required by Entity Framework Core.
    /// </summary>
    protected Quote() { }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Quote"/> class.
    ///     Sets the initial status to DRAFT.
    /// </summary>
    /// <param name="workOrderId">The associated work order ID.</param>
    /// <param name="branchId">The branch ID where it's created.</param>
    /// <param name="subtotalAmount">The initial subtotal amount.</param>
    /// <param name="discountPercentage">The initial discount percentage.</param>
    public Quote(Guid workOrderId, Guid branchId, decimal subtotalAmount, decimal discountPercentage)
    {
        Id = Guid.NewGuid();
        WorkOrderId = workOrderId;
        BranchId = branchId;
        SubtotalAmount = subtotalAmount;
        DiscountPercentage = discountPercentage;
        Status = QuoteStatus.DRAFT;
        CreatedBy = Guid.Empty;
        
        CalculateFinalTotal();
    }

    /// <summary>
    ///     Applies a new discount percentage and recalculates the total amount.
    /// </summary>
    /// <param name="discountPercentage">The new discount percentage to apply.</param>
    public void ApplyDiscount(decimal discountPercentage)
    {
        DiscountPercentage = discountPercentage;
        CalculateFinalTotal();
    }

    /// <summary>
    ///     Updates the subtotal and discount, then recalculates the total amount.
    /// </summary>
    /// <param name="subtotalAmount">The new subtotal amount.</param>
    /// <param name="discountPercentage">The new discount percentage.</param>
    public void Update(decimal subtotalAmount, decimal discountPercentage)
    {
        SubtotalAmount = subtotalAmount;
        DiscountPercentage = discountPercentage;
        CalculateFinalTotal();
    }

    /// <summary>
    ///     Approves the quote, allowing it to be converted into a voucher.
    /// </summary>
    public void Approve()
    {
        Status = QuoteStatus.APPROVED;
    }

    /// <summary>
    ///     Cancels the quote, preventing it from being processed further.
    /// </summary>
    public void Cancel()
    {
        Status = QuoteStatus.CANCELLED;
    }

    /// <summary>
    ///     Calculates the final total amount based on the subtotal and discount.
    /// </summary>
    private void CalculateFinalTotal()
    {
        var discountMultiplier = 1m - (DiscountPercentage / 100m);
        TotalAmount = SubtotalAmount * discountMultiplier;
    }
}
