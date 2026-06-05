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
            b.Property(b => b.Value).HasColumnName("branch_id").IsRequired();
        });

        builder.OwnsOne(p => p.Category, c =>
        {
            c.Property(c => c.Value).HasColumnName("category").IsRequired();
        });

        builder.OwnsOne(p => p.Name, n =>
        {
            n.Property(n => n.Name).HasColumnName("name").IsRequired();
        });

        builder.OwnsOne(p => p.Sku, s =>
        {
            s.Property(s => s.Value).HasColumnName("sku").IsRequired();
        });

        builder.Property(p => p.Description).IsRequired().HasColumnName("description");

        builder.OwnsOne(p => p.CurrentSellingPrice, m =>
        {
            m.Property(m => m.Amount).HasColumnName("current_selling_price").HasColumnType("decimal(18,2)").IsRequired();
        });

        builder.OwnsOne(p => p.CurrentStock, s =>
        {
            s.Property(s => s.Value).HasColumnName("current_stock").IsRequired();
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
