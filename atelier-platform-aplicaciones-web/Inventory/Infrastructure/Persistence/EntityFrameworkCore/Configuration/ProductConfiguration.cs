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

        builder.Property(p => p.BranchId)
            .HasConversion(v => v.Value, v => new atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects.BranchId(v))
            .HasColumnName("branch_id")
            .IsRequired();

        builder.Property(p => p.Category)
            .HasConversion(v => v.Value, v => new atelier_platform_aplicaciones_web.Inventory.Domain.Model.ValueObjects.ProductCategory(v))
            .HasColumnName("category")
            .IsRequired();

        builder.Property(p => p.Name)
            .HasConversion(v => v.Name, v => new atelier_platform_aplicaciones_web.Inventory.Domain.Model.ValueObjects.ProductName(v))
            .HasColumnName("name")
            .IsRequired();

        builder.Property(p => p.Sku)
            .HasConversion(v => v.Value, v => new atelier_platform_aplicaciones_web.Inventory.Domain.Model.ValueObjects.Sku(v))
            .HasColumnName("sku")
            .IsRequired();

        builder.Property(p => p.Description).IsRequired().HasColumnName("description");

        builder.Property(p => p.CurrentSellingPrice)
            .HasConversion(v => v.Amount, v => new atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects.Money(v))
            .HasColumnName("current_selling_price")
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(p => p.CurrentStock)
            .HasConversion(v => v.Value, v => new atelier_platform_aplicaciones_web.Inventory.Domain.Model.ValueObjects.InventoryQuantity(v))
            .HasColumnName("current_stock")
            .IsRequired();

        builder.Property(p => p.MinimumStock)
            .IsRequired()
            .HasColumnName("minimum_stock");

        builder.HasMany(p => p.Batches)
            .WithOne()
            .HasForeignKey(b => b.ProductId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
