using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Inventory.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Inventory.Domain.Model.Queries;

namespace atelier_platform_aplicaciones_web.Inventory.Application.QueryServices;

public interface IProductQueryService
{
    Task<Product?> Handle(GetProductByIdQuery query, CancellationToken cancellationToken = default);
    Task<System.Collections.Generic.IEnumerable<Product>> Handle(GetProductsByBranchIdQuery query, CancellationToken cancellationToken = default);
}
