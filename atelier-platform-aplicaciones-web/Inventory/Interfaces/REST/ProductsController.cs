using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using atelier_platform_aplicaciones_web.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using atelier_platform_aplicaciones_web.Inventory.Application.CommandServices;
using atelier_platform_aplicaciones_web.Inventory.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.Inventory.Interfaces.REST.Transform;

namespace atelier_platform_aplicaciones_web.Inventory.Interfaces.REST;

[ApiController]
[Route("api/v1/inventory/products")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Inventory Products")]
[Authorize]
public class ProductsController(
    IProductCommandService productCommandService,
    atelier_platform_aplicaciones_web.Inventory.Application.QueryServices.IProductQueryService _productQueryService) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new Product")]
    public async Task<ActionResult> CreateProduct([FromBody] CreateProductResource resource)
    {
        var command = ProductCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await productCommandService.Handle(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result.Message);
        }

        if (result.Value == null)
        {
            return BadRequest("Product could not be created.");
        }

        var productResource = ProductResourceFromEntityAssembler.ToResourceFromEntity(result.Value);
        return CreatedAtAction(nameof(GetProductById), new { id = result.Value.Id }, productResource);
    }

    [HttpGet("{id}")]
    [SwaggerOperation(Summary = "Get a Product by ID")]
    public async Task<ActionResult> GetProductById(System.Guid id)
    {
        var query = new atelier_platform_aplicaciones_web.Inventory.Domain.Model.Queries.GetProductByIdQuery(id);
        var product = await _productQueryService.Handle(query);
        
        if (product == null)
        {
            return NotFound();
        }

        var productResource = ProductDetailsResourceFromEntityAssembler.ToResourceFromEntity(product);
        return Ok(productResource);
    }

    [HttpPost("{id}/batches")]
    [SwaggerOperation(Summary = "Add a Batch to a Product")]
    public async Task<ActionResult> AddBatch(System.Guid id, [FromBody] AddBatchToProductResource resource)
    {
        var command = AddBatchToProductCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await productCommandService.Handle(command);

        if (!result.IsSuccess)
        {
            if (result.Error != null && result.Error.Equals(atelier_platform_aplicaciones_web.Inventory.Domain.Model.InventoryError.NotFound))
            {
                return NotFound();
            }
            return BadRequest(result.Message);
        }

        if (result.Value == null)
        {
            return BadRequest("Batch could not be added.");
        }

        var productResource = ProductDetailsResourceFromEntityAssembler.ToResourceFromEntity(result.Value);
        return Ok(productResource);
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update a Product")]
    public async Task<ActionResult> UpdateProduct(System.Guid id, [FromBody] UpdateProductResource resource)
    {
        var command = UpdateProductCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await productCommandService.Handle(command);

        if (!result.IsSuccess)
        {
            if (result.Error != null && result.Error.Equals(atelier_platform_aplicaciones_web.Inventory.Domain.Model.InventoryError.NotFound))
            {
                return NotFound();
            }
            return BadRequest(result.Message);
        }

        if (result.Value == null)
        {
            return BadRequest("Product could not be updated.");
        }

        var productResource = ProductDetailsResourceFromEntityAssembler.ToResourceFromEntity(result.Value);
        return Ok(productResource);
    }

    [HttpGet("branch/{branchId}")]
    [SwaggerOperation(Summary = "Get Products by Branch ID")]
    public async Task<ActionResult> GetProductsByBranchId(System.Guid branchId)
    {
        var query = new atelier_platform_aplicaciones_web.Inventory.Domain.Model.Queries.GetProductsByBranchIdQuery(branchId);
        var products = await _productQueryService.Handle(query);
        var resources = products.Select(ProductResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpDelete("{id}")]
    [SwaggerOperation(Summary = "Delete a Product")]
    public async Task<ActionResult> DeleteProduct(System.Guid id)
    {
        var command = new atelier_platform_aplicaciones_web.Inventory.Domain.Model.Commands.DeleteProductCommand(id);
        var result = await productCommandService.Handle(command);

        if (!result.IsSuccess)
        {
            if (result.Error != null && result.Error.Equals(atelier_platform_aplicaciones_web.Inventory.Domain.Model.InventoryError.NotFound))
            {
                return NotFound();
            }
            return BadRequest(result.Message);
        }

        return NoContent();
    }
}
