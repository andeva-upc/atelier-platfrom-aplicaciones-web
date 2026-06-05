using System;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Inventory.Application.CommandServices;
using atelier_platform_aplicaciones_web.Inventory.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Inventory.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Inventory.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Inventory.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Application.Model;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.Inventory.Application.Internal.CommandServices;

public class ProductCommandService : IProductCommandService
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ProductCommandService(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<Product>> Handle(CreateProductCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var product = new Product(
                new BranchId(command.BranchId),
                new ProductCategory(command.Category),
                new ProductName(command.Name),
                new Sku(command.Sku),
                command.Description,
                new Money(command.SalePrice),
                command.MinimumStock
            );

            await _productRepository.AddAsync(product, cancellationToken);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result<Product>.Success(product);
        }
        catch (Exception ex)
        {
            return Result<Product>.Failure(atelier_platform_aplicaciones_web.Inventory.Domain.Model.InventoryError.UnexpectedError, "An unexpected error occurred while creating the product: " + ex.Message);
        }
    }

    public async Task<Result<Product>> Handle(UpdateProductCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var product = await _productRepository.FindByIdAsync(command.ProductId, cancellationToken);
            if (product == null)
            {
                return Result<Product>.Failure(atelier_platform_aplicaciones_web.Inventory.Domain.Model.InventoryError.NotFound, "Product not found.");
            }

            product.UpdateInformation(
                new ProductCategory(command.Category),
                new ProductName(command.Name),
                new Sku(command.Sku),
                command.Description,
                new Money(command.SalePrice),
                command.MinimumStock
            );

            _productRepository.Update(product);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result<Product>.Success(product);
        }
        catch (Exception ex)
        {
            return Result<Product>.Failure(atelier_platform_aplicaciones_web.Inventory.Domain.Model.InventoryError.UnexpectedError, "An unexpected error occurred while updating the product: " + ex.Message);
        }
    }

    public async Task<Result<Product>> Handle(AddBatchToProductCommand command, CancellationToken cancellationToken = default)
    {
        try
        {
            var product = await _productRepository.FindByIdAsync(command.ProductId, cancellationToken);
            if (product == null)
            {
                return Result<Product>.Failure(atelier_platform_aplicaciones_web.Inventory.Domain.Model.InventoryError.NotFound, "Product not found.");
            }

            product.AddBatch(command.Quantity, command.Description);

            _productRepository.Update(product);
            await _unitOfWork.CompleteAsync(cancellationToken);

            return Result<Product>.Success(product);
        }
        catch (Exception ex)
        {
            return Result<Product>.Failure(atelier_platform_aplicaciones_web.Inventory.Domain.Model.InventoryError.UnexpectedError, "An unexpected error occurred while adding the batch: " + ex.Message);
        }
    }
}
