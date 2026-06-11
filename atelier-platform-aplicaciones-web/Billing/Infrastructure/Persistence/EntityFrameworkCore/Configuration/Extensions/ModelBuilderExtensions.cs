using atelier_platform_aplicaciones_web.Billing.Domain.Model.Aggregates;
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
            
            entity.Property(e => e.WorkOrderId).HasColumnName("WorkOrderId").IsRequired();
            entity.Property(e => e.BranchId).HasColumnName("BranchId").IsRequired();
            entity.Property(e => e.SubtotalAmount).HasColumnName("SubtotalAmount").HasColumnType("decimal(10,2)").IsRequired();
            entity.Property(e => e.DiscountPercentage).HasColumnName("DiscountPercentage").HasColumnType("decimal(5,2)").IsRequired();
            entity.Property(e => e.TotalAmount).HasColumnName("TotalAmount").HasColumnType("decimal(10,2)").IsRequired();

            entity.Property(e => e.Status).HasConversion<string>().HasColumnName("Status").HasMaxLength(20).IsRequired();
        });

        builder.Entity<Voucher>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).ValueGeneratedNever();
            
            entity.Property(e => e.QuoteId).HasColumnName("quote_id").IsRequired();
            entity.Property(e => e.BranchId).HasColumnName("branch_id").IsRequired();
            entity.Property(e => e.VoucherNumber).HasColumnName("voucher_number").IsRequired();
            entity.Property(e => e.SubtotalAmount).HasColumnName("subtotal_amount").HasColumnType("decimal(10,2)").IsRequired();
            entity.Property(e => e.TotalAmount).HasColumnName("total_amount").HasColumnType("decimal(10,2)").IsRequired();
            entity.Property(e => e.Type).HasConversion<string>().HasColumnName("type").HasMaxLength(20).IsRequired();
            entity.Property(e => e.Status).HasConversion<string>().HasColumnName("status").HasMaxLength(20).IsRequired();
            entity.Property(e => e.Currency).HasConversion<string>().HasColumnName("currency").HasMaxLength(3).IsRequired();

            entity.Property(e => e.CustomerDocumentType).HasColumnName("customer_document_type");
            entity.Property(e => e.CustomerDocumentNumber).HasColumnName("customer_document_number");
            entity.Property(e => e.CustomerName).HasColumnName("customer_name");
            entity.Property(e => e.ExternalInvoiceId).HasColumnName("external_invoice_id");
        });

        return builder;
    }
}
