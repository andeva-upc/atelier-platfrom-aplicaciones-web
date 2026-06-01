using atelier_platform_aplicaciones_web.Inventory.Domain.Model.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace atelier_platform_aplicaciones_web.Inventory.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

public static class InventoryContextConfigurationExtensions
{
    public static ModelBuilder ApplyInventoryConfiguration(this ModelBuilder builder)
    {
        builder.Entity<Product>();
        builder.ApplyConfiguration(new ProductConfiguration());

        return builder;
    }
}
