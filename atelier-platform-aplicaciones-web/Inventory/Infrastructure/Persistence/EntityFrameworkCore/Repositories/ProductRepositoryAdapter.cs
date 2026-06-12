using System;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Inventory.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Inventory.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

namespace atelier_platform_aplicaciones_web.Inventory.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ProductRepositoryAdapter : BaseRepository<Product>, IProductRepository
{
    public ProductRepositoryAdapter(AppDbContext context) : base(context)
    {
    }

    public new async Task AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        await Context.Products.AddAsync(product, cancellationToken);
    }
}
