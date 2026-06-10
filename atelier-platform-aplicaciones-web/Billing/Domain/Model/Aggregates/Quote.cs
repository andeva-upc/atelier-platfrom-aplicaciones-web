using System.Collections.Generic;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.Entities;

namespace atelier_platform_aplicaciones_web.Billing.Domain.Model.Aggregates;

public class Quote : IAuditableEntity
{
    public Guid Id { get; private set; }
    public Guid WorkshopId { get; private set; }
    public Guid CustomerId { get; private set; }
    public Guid VehicleId { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public string Currency { get; private set; } = string.Empty;
    public Money Subtotal { get; private set; }
    public Money DiscountAmount { get; private set; }
    public TaxRate TaxRate { get; private set; }
    public Money Total { get; private set; }
    public QuoteStatus Status { get; private set; }

    // Navigation property
    public ICollection<QuoteItem> Items { get; private set; }

    // IAuditableEntity properties
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }

    protected Quote() { }

    public Quote(Guid workshopId, Guid customerId, Guid vehicleId, string description, string currency, Money subtotal, TaxRate taxRate)
    {
        Id = Guid.NewGuid();
        WorkshopId = workshopId;
        CustomerId = customerId;
        VehicleId = vehicleId;
        Description = description;
        Currency = currency;
        Subtotal = subtotal;
        TaxRate = taxRate;
        DiscountAmount = new Money(0, currency);
        Status = QuoteStatus.DRAFT;
        Items = new List<QuoteItem>();
        
        CalculateFinalTotal();
    }

    public void AddItem(string type, Guid referenceId, string description, decimal quantity, Money unitPrice)
    {
        var item = new QuoteItem(this.Id, type, referenceId, description, quantity, unitPrice);
        Items.Add(item);
        
        Subtotal = Subtotal.Add(item.TotalPrice);
        CalculateFinalTotal();
    }

    public void ApplyDiscount(Money amount)
    {
        if (amount.Currency != Currency)
            throw new InvalidOperationException("Discount currency must match quote currency.");
            
        DiscountAmount = amount;
        CalculateFinalTotal();
    }

    public void Approve()
    {
        Status = QuoteStatus.APPROVED;
    }

    public void Cancel()
    {
        Status = QuoteStatus.CANCELLED;
    }

    private void CalculateFinalTotal()
    {
        // Total = (Subtotal - Discount) + Tax
        var discountedSubtotalAmount = Subtotal.Amount - DiscountAmount.Amount;
        if (discountedSubtotalAmount < 0) discountedSubtotalAmount = 0;

        var discountedSubtotal = new Money(discountedSubtotalAmount, Currency);
        var taxAmount = TaxRate.CalculateTax(discountedSubtotal);
        Total = discountedSubtotal.Add(taxAmount);
    }
}
