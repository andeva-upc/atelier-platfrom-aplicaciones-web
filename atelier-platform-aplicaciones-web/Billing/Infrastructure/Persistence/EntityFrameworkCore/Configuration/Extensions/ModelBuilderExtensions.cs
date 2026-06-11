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

        return builder;
    }
}
