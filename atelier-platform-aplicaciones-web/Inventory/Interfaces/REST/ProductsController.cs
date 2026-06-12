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
public class ProductsController(IProductCommandService productCommandService) : ControllerBase
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
        // Returns 201 Created. The URL will be added properly when we implement GetProductById
        return Created(string.Empty, productResource);
    }
}
