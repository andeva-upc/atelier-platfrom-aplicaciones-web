using System;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Core.Application.CommandServices;
using atelier_platform_aplicaciones_web.Core.Application.QueryServices;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Core.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.Core.Interfaces.REST.Transform;
using atelier_platform_aplicaciones_web.Core.Resources;
using atelier_platform_aplicaciones_web.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace atelier_platform_aplicaciones_web.Core.Interfaces.REST;

[ApiController]
[Route("api/v1/customers")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Customers")]
[Authorize]
public class CustomersController(
    ICustomerCommandService customerCommandService,
    ICustomerQueryService customerQueryService,
    IStringLocalizer<CoreMessages> localizer) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new customer profile", Description = "Creates a new customer profile associated with a user ID")]
    public async Task<ActionResult> CreateCustomer([FromBody] CreateCustomerResource resource, CancellationToken cancellationToken)
    {
        var command = CreateCustomerCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await customerCommandService.Handle(command, cancellationToken);

        return ActionResultFromCoreCommandResultAssembler.ToCreatedAtActionResult(
            result,
            CustomerResourceFromEntityAssembler.ToResourceFromEntity,
            this,
            localizer,
            nameof(GetCustomerById),
            "customerId"
        );
    }

    [HttpPut("user/{userId}")]
    [SwaggerOperation(Summary = "Update a customer profile", Description = "Updates an existing customer profile using the user ID")]
    public async Task<ActionResult> UpdateCustomer(Guid userId, [FromBody] UpdateCustomerResource resource, CancellationToken cancellationToken)
    {
        var command = UpdateCustomerCommandFromResourceAssembler.ToCommandFromResource(userId, resource);
        var result = await customerCommandService.Handle(command, cancellationToken);

        return ActionResultFromCoreCommandResultAssembler.ToOkActionResult(
            result,
            CustomerResourceFromEntityAssembler.ToResourceFromEntity,
            this,
            localizer
        );
    }

    [HttpGet("{customerId}")]
    [ActionName(nameof(GetCustomerById))]
    [SwaggerOperation(Summary = "Get a customer profile by ID", Description = "Retrieves the details of a specific customer profile")]
    public async Task<ActionResult> GetCustomerById(Guid customerId, CancellationToken cancellationToken)
    {
        var query = new GetCustomerByIdQuery(new CustomerId(customerId));
        var customer = await customerQueryService.Handle(query, cancellationToken);

        if (customer == null) return NotFound();

        return Ok(CustomerResourceFromEntityAssembler.ToResourceFromEntity(customer));
    }

    [HttpDelete("user/{userId}")]
    [SwaggerOperation(Summary = "Delete a customer profile", Description = "Deletes an existing customer profile using the user ID")]
    public async Task<ActionResult> DeleteCustomer(Guid userId, CancellationToken cancellationToken)
    {
        var command = new DeleteCustomerCommand(new UserId(userId));
        var result = await customerCommandService.Handle(command, cancellationToken);

        return ActionResultFromCoreCommandResultAssembler.ToNoContentActionResult(
            result,
            this,
            localizer
        );
    }
}
