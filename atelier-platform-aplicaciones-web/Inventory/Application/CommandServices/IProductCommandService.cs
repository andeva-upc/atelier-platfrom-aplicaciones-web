using System;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Inventory.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Inventory.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Shared.Application.Model;

namespace atelier_platform_aplicaciones_web.Inventory.Application.CommandServices;

public interface IProductCommandService
{
    Task<Result<Product>> Handle(CreateProductCommand command, CancellationToken cancellationToken = default);
    Task<Result<Product>> Handle(UpdateProductCommand command, CancellationToken cancellationToken = default);
}
