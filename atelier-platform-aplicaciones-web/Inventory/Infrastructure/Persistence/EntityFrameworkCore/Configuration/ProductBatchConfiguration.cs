using atelier_platform_aplicaciones_web.Inventory.Domain.Model.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace atelier_platform_aplicaciones_web.Inventory.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class ProductBatchConfiguration : IEntityTypeConfiguration<ProductBatch>
{
    public void Configure(EntityTypeBuilder<ProductBatch> builder)
    {
        builder.ToTable("product_batches");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .IsRequired()
            .ValueGeneratedNever()
            .HasColumnName("id");

        builder.Property(b => b.ProductId)
            .IsRequired()
            .HasColumnName("product_id");

        builder.OwnsOne(b => b.Quantity, q =>
        {
            q.Property("ProductBatchId").HasColumnName("id");
            q.Property(v => v.Value)
                .IsRequired()
                .HasColumnName("initial_quantity");
        });

        builder.OwnsOne(b => b.AvailableQuantity, a =>
        {
            a.Property("ProductBatchId").HasColumnName("id");
            a.Property(v => v.Value)
                .IsRequired()
                .HasColumnName("available_quantity");
        });

        builder.OwnsOne(b => b.AcquisitionCost, c =>
        {
            c.Property("ProductBatchId").HasColumnName("id");
            c.Property(v => v.Amount)
                .IsRequired()
                .HasColumnName("acquisition_cost")
                .HasColumnType("decimal(10,2)");
        });

        builder.Property(b => b.CreatedAt)
            .IsRequired()
            .HasColumnName("created_at");

        builder.Ignore(b => b.ReservedQuantity);
    }
}
