using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
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

    public new async Task<Product?> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await Context.Products
            .Include(p => p.Batches)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Product>> FindAllByBranchIdAsync(Guid branchId)
    {
        var branchIdObj = new atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects.BranchId(branchId);
        return await Context.Products
            .Where(p => p.BranchId == branchIdObj)
            .ToListAsync();
    }
}
