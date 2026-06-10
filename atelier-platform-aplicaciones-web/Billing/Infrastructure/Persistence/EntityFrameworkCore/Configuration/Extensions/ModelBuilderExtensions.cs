using atelier_platform_aplicaciones_web.Billing.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace atelier_platform_aplicaciones_web.Billing.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class ModelBuilderExtensions
{
    public static ModelBuilder ApplyBillingConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Quote>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            
            entity.OwnsOne(e => e.Subtotal, subtotal =>
            {
                subtotal.Property(m => m.Amount).HasColumnName("SubtotalAmount");
                subtotal.Property(m => m.Currency).HasColumnName("Currency");
            });

            entity.OwnsOne(e => e.DiscountAmount, discount =>
            {
                discount.Property(m => m.Amount).HasColumnName("DiscountAmount");
            });

            entity.OwnsOne(e => e.TaxRate, tax =>
            {
                tax.Property(t => t.Percentage).HasColumnName("TaxPercentage");
            });

            entity.OwnsOne(e => e.Total, total =>
            {
                total.Property(m => m.Amount).HasColumnName("TotalAmount");
            });

            entity.Property(e => e.Status).HasConversion<string>();

            entity.HasMany(e => e.Items)
                  .WithOne()
                  .HasForeignKey(i => i.QuoteId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        builder.Entity<QuoteItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.OwnsOne(e => e.UnitPrice, price =>
            {
                price.Property(m => m.Amount).HasColumnName("UnitPriceAmount");
                price.Property(m => m.Currency).HasColumnName("Currency");
            });

            entity.OwnsOne(e => e.TotalPrice, price =>
            {
                price.Property(m => m.Amount).HasColumnName("TotalPriceAmount");
            });
        });

        return builder;
    }
}
