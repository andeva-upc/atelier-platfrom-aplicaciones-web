using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;
using atelier_platform_aplicaciones_web.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using atelier_platform_aplicaciones_web.Operations.Application.CommandServices;
using atelier_platform_aplicaciones_web.Operations.Application.QueryServices;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Operations.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.Operations.Interfaces.REST.Transform;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Operations.Resources;

namespace atelier_platform_aplicaciones_web.Operations.Interfaces.REST;

[ApiController]
[Route("api/v1/services")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Services")]
[Authorize]
public class ServicesController(
    IServiceCommandService serviceCommandService,
    IServiceQueryService serviceQueryService,
    IStringLocalizer<OperationsMessages> localizer)
    : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new service")]
    public async Task<ActionResult> CreateService([FromBody] CreateServiceResource resource)
    {
        var command = CreateServiceCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await serviceCommandService.Handle(command);

        return ActionResultFromServiceCommandResultAssembler.ToCreatedAtActionResult(
            result,
            this,
            localizer,
            nameof(GetServiceById));
    }

    [HttpPut("service/{serviceId}")]
    [SwaggerOperation(Summary = "Update a service")]
    public async Task<ActionResult> UpdateService(Guid serviceId, [FromBody] UpdateServiceResource resource)
    {
        var command = UpdateServiceCommandFromResourceAssembler.ToCommandFromResource(serviceId, resource);
        var result = await serviceCommandService.Handle(command);

        return ActionResultFromServiceCommandResultAssembler.ToOkActionResult(result, this, localizer);
    }

    [HttpDelete("service/{serviceId}")]
    [SwaggerOperation(Summary = "Delete a service")]
    public async Task<ActionResult> DeleteService(Guid serviceId)
    {
        var command = new DeleteServiceCommand(new ServiceId(serviceId));
        var result = await serviceCommandService.Handle(command);

        return ActionResultFromServiceCommandResultAssembler.ToNoContentActionResult(result, this, localizer);
    }

    [HttpGet("service/{branchId}")]
    [SwaggerOperation(Summary = "Get all services for a specific branch")]
    public async Task<ActionResult> GetServicesByBranch(Guid branchId)
    {
        var query = new GetAllServicesByBranchIdQuery(new BranchId(branchId));
        var services = await serviceQueryService.Handle(query);
        var resources = services.Select(ServiceResourceFromEntityAssembler.ToResourceFromEntity);

        return Ok(resources);
    }

    [HttpGet("service/id/{serviceId}")]
    [ActionName(nameof(GetServiceById))]
    [SwaggerOperation(Summary = "Get a service by ID")]
    public async Task<ActionResult> GetServiceById(Guid serviceId)
    {
        var query = new GetServiceByIdQuery(new ServiceId(serviceId));
        var service = await serviceQueryService.Handle(query);

        if (service == null) return NotFound();

        return Ok(ServiceResourceFromEntityAssembler.ToResourceFromEntity(service));
    }
}
