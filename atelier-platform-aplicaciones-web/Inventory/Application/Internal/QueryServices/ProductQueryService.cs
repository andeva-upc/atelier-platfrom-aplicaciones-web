using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Inventory.Application.QueryServices;
using atelier_platform_aplicaciones_web.Inventory.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Inventory.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.Inventory.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.Inventory.Application.Internal.QueryServices;

public class ProductQueryService : IProductQueryService
{
    private readonly IProductRepository _productRepository;

    public ProductQueryService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Product?> Handle(GetProductByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _productRepository.FindByIdAsync(query.ProductId, cancellationToken);
    }

    public async Task<System.Collections.Generic.IEnumerable<Product>> Handle(GetProductsByBranchIdQuery query, CancellationToken cancellationToken = default)
    {
        return await _productRepository.FindAllByBranchIdAsync(query.BranchId);
    }
}
