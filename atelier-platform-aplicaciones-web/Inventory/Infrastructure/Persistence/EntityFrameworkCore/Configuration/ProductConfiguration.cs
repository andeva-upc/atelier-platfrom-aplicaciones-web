using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using atelier_platform_aplicaciones_web.Inventory.Domain.Model.Aggregates;

namespace atelier_platform_aplicaciones_web.Inventory.Infrastructure.Persistence.EntityFrameworkCore.Configuration;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("products");
        builder.HasKey(p => p.Id);
        
        builder.Property(p => p.Id).IsRequired().ValueGeneratedNever().HasColumnName("id");

        builder.OwnsOne(p => p.BranchId, b =>
        {
            b.Property("ProductId").HasColumnName("id");
            b.Property(b => b.Value).HasColumnName("BranchId").IsRequired();
        });

        builder.OwnsOne(p => p.Category, c =>
        {
            c.Property("ProductId").HasColumnName("id");
            c.Property(c => c.Value).HasColumnName("Category").IsRequired();
        });

        builder.OwnsOne(p => p.Name, n =>
        {
            n.Property("ProductId").HasColumnName("id");
            n.Property(n => n.Name).HasColumnName("Name").IsRequired();
        });

        builder.OwnsOne(p => p.Sku, s =>
        {
            s.Property("ProductId").HasColumnName("id");
            s.Property(s => s.Value).HasColumnName("Sku").IsRequired();
        });

        builder.Property(p => p.Description).IsRequired().HasColumnName("description");

        builder.OwnsOne(p => p.CurrentSellingPrice, m =>
        {
            m.Property("ProductId").HasColumnName("id");
            m.Property(m => m.Amount).HasColumnName("CurrentSellingPrice").HasColumnType("decimal(18,2)").IsRequired();
        });

        builder.OwnsOne(p => p.CurrentStock, s =>
        {
            s.Property("ProductId").HasColumnName("id");
            s.Property(s => s.Value).HasColumnName("CurrentStock").IsRequired();
        });

        builder.Property(p => p.MinimumStock)
            .IsRequired()
            .HasColumnName("minimum_stock");

        builder.HasMany(p => p.Batches)
            .WithOne()
            .HasForeignKey(b => b.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
