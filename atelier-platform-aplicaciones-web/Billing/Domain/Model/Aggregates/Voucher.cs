using System;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.Entities;

namespace atelier_platform_aplicaciones_web.Billing.Domain.Model.Aggregates;

/// <summary>
///     Represents a Billing Voucher (Boleta/Factura) generated after a Work Order is completed.
///     Acts as an Aggregate Root in the Billing context.
/// </summary>
public class Voucher : IUserAuditableEntity
{
    /// <summary>
    ///     Unique identifier for the voucher.
    /// </summary>
    public Guid Id { get; private set; }

    /// <summary>
    ///     Identifier of the quote that generated this voucher.
    /// </summary>
    public Guid QuoteId { get; private set; }

    /// <summary>
    ///     Identifier of the branch where this voucher was issued.
    /// </summary>
    public Guid BranchId { get; private set; }

    /// <summary>
    ///     Sequential number of the voucher.
    /// </summary>
    public int VoucherNumber { get; private set; }

    /// <summary>
    ///     Subtotal amount of the voucher before taxes.
    /// </summary>
    public decimal SubtotalAmount { get; private set; }

    /// <summary>
    ///     Total amount of the voucher including taxes.
    /// </summary>
    public decimal TotalAmount { get; private set; }

    /// <summary>
    ///     Type of the voucher (e.g., INVOICE, RECEIPT).
    /// </summary>
    public string Type { get; private set; }

    /// <summary>
    ///     Current status of the voucher (e.g., PENDING, PAID, CANCELED).
    /// </summary>
    public string Status { get; private set; }

    /// <summary>
    ///     Currency of the voucher (e.g., PEN, USD).
    /// </summary>
    public string Currency { get; private set; }
    
    /// <summary>
    ///     Customer's identity document type (e.g., DNI, RUC).
    /// </summary>
    public string? CustomerDocumentType { get; private set; }

    /// <summary>
    ///     Customer's identity document number.
    /// </summary>
    public string? CustomerDocumentNumber { get; private set; }

    /// <summary>
    ///     Customer's full name or business name.
    /// </summary>
    public string? CustomerName { get; private set; }

    /// <summary>
    ///     External ID returned by the electronic billing provider (e.g., Facthub).
    /// </summary>
    public Guid? ExternalInvoiceId { get; private set; }

    /// <summary>
    ///     Collection of payments associated with this voucher.
    /// </summary>
    public ICollection<atelier_platform_aplicaciones_web.Billing.Domain.Model.Entities.Payment> Payments { get; private set; }

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
    protected Voucher()
    {
        Type = null!;
        Status = null!;
        Currency = null!;
        Payments = new System.Collections.Generic.List<atelier_platform_aplicaciones_web.Billing.Domain.Model.Entities.Payment>();
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Voucher"/> class.
    /// </summary>
    /// <param name="quoteId">The associated quote ID.</param>
    /// <param name="branchId">The branch ID where it's generated.</param>
    /// <param name="voucherNumber">The sequential document number.</param>
    /// <param name="subtotalAmount">The total before taxes.</param>
    /// <param name="type">The type of the voucher (e.g. INVOICE).</param>
    /// <param name="currency">The currency (e.g. PEN).</param>
    /// <param name="customerDocType">Customer's document type.</param>
    /// <param name="customerDocNum">Customer's document number.</param>
    /// <param name="customerName">Customer's name.</param>
    /// <param name="externalInvoiceId">Facthub external reference ID.</param>
    public Voucher(Guid quoteId, Guid branchId, int voucherNumber, decimal subtotalAmount, string type, string currency, string? customerDocType, string? customerDocNum, string? customerName, Guid? externalInvoiceId)
    {
        Id = Guid.NewGuid();
        QuoteId = quoteId;
        BranchId = branchId;
        VoucherNumber = voucherNumber;
        SubtotalAmount = subtotalAmount;
        Type = type;
        Currency = currency;
        CustomerDocumentType = customerDocType;
        CustomerDocumentNumber = customerDocNum;
        CustomerName = customerName;
        ExternalInvoiceId = externalInvoiceId;
        Status = VoucherStatus.PENDING;
        CreatedBy = Guid.Empty;
        Payments = new System.Collections.Generic.List<atelier_platform_aplicaciones_web.Billing.Domain.Model.Entities.Payment>();
        CalculateTotal();
    }

    /// <summary>
    ///     Calculates the total amount including a standard 18% tax rate.
    /// </summary>
    private void CalculateTotal()
    {
        // Total is Subtotal + 18% IGV/Tax
        TotalAmount = SubtotalAmount * 1.18m;
    }

    /// <summary>
    ///     Adds a payment to the voucher and updates its status to PAID if fully covered.
    /// </summary>
    /// <param name="amount">The amount paid.</param>
    /// <param name="method">The payment method (e.g. CASH, CARD).</param>
    /// <param name="currency">The currency used.</param>
    /// <exception cref="Exception">Thrown if payment exceeds balance or voucher is canceled.</exception>
    public void AddPayment(decimal amount, string method, string currency)
    {
        if (Status == VoucherStatus.PAID)
            throw new Exception("Voucher is already fully paid.");

        if (Status == VoucherStatus.CANCELED)
            throw new Exception("Cannot add payment to a canceled voucher.");

        var totalPaid = System.Linq.Enumerable.Sum(Payments, p => p.Amount);
        var remaining = TotalAmount - totalPaid;

        if (amount > remaining)
            throw new Exception("Payment amount exceeds the remaining balance.");

        var payment = new atelier_platform_aplicaciones_web.Billing.Domain.Model.Entities.Payment(Id, BranchId, amount, method, currency);
        Payments.Add(payment);

        if (totalPaid + amount >= TotalAmount)
        {
            Status = VoucherStatus.PAID;
        }
    }

    /// <summary>
    ///     Removes a registered payment from the voucher.
    ///     If the voucher was PAID, it reverts its status to PENDING.
    /// </summary>
    /// <param name="paymentId">The unique ID of the payment to remove.</param>
    /// <exception cref="Exception">Thrown if the payment is not found.</exception>
    public void RemovePayment(Guid paymentId)
    {
        var payment = System.Linq.Enumerable.FirstOrDefault(Payments, p => p.Id == paymentId);
        if (payment == null)
            throw new Exception("Payment not found.");

        Payments.Remove(payment);

        var totalPaid = System.Linq.Enumerable.Sum(Payments, p => p.Amount);
        if (totalPaid < TotalAmount && Status == VoucherStatus.PAID)
        {
            Status = VoucherStatus.PENDING;
        }
    }

    /// <summary>
    ///     Forces the voucher status to PAID.
    /// </summary>
    public void MarkAsPaid()
    {
        Status = VoucherStatus.PAID;
    }
}
