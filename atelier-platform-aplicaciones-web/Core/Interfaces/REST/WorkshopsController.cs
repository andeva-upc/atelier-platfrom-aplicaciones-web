using System;
using System.Collections.Generic;
using System.Linq;
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace atelier_platform_aplicaciones_web.Core.Interfaces.REST;

[ApiController]
[Route("api/v1/workshops")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Workshops")]
[Authorize]
public class WorkshopsController(
    IWorkshopCommandService workshopCommandService,
    IWorkshopQueryService workshopQueryService,
    IStringLocalizer<CoreMessages> localizer) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new workshop", Description = "Creates a new workshop")]
    public async Task<ActionResult> CreateWorkshop([FromBody] CreateWorkshopResource resource, CancellationToken cancellationToken)
    {
        var command = CreateWorkshopCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await workshopCommandService.Handle(command, cancellationToken);

        return ActionResultFromCoreCommandResultAssembler.ToCreatedAtActionResult(
            result,
            WorkshopResourceFromEntityAssembler.ToResourceFromEntity,
            this,
            localizer,
            nameof(GetWorkshopById),
            "workshopId"
        );
    }

    [HttpPut("{workshopId}")]
    [SwaggerOperation(Summary = "Update an existing workshop", Description = "Updates the details of a workshop by its ID")]
    public async Task<ActionResult> UpdateWorkshop(Guid workshopId, [FromBody] UpdateWorkshopResource resource, CancellationToken cancellationToken)
    {
        var command = UpdateWorkshopCommandFromResourceAssembler.ToCommandFromResource(workshopId, resource);
        var result = await workshopCommandService.Handle(command, cancellationToken);

        return ActionResultFromCoreCommandResultAssembler.ToOkActionResult(
            result,
            WorkshopResourceFromEntityAssembler.ToResourceFromEntity,
            this,
            localizer
        );
    }

    [HttpGet("{workshopId}")]
    [ActionName(nameof(GetWorkshopById))]
    [SwaggerOperation(Summary = "Get a workshop by ID", Description = "Retrieves the details of a specific workshop")]
    public async Task<ActionResult> GetWorkshopById(Guid workshopId, CancellationToken cancellationToken)
    {
        var query = new GetWorkshopByIdQuery(new WorkshopId(workshopId));
        var workshop = await workshopQueryService.Handle(query, cancellationToken);

        if (workshop == null) return NotFound();

        return Ok(WorkshopResourceFromEntityAssembler.ToResourceFromEntity(workshop));
    }

    [HttpGet("owner/{ownerId}")]
    [SwaggerOperation(Summary = "Get workshops by owner ID", Description = "Retrieves all workshops belonging to a specific owner")]
    public async Task<ActionResult> GetWorkshopsByOwnerId(Guid ownerId, CancellationToken cancellationToken)
    {
        var query = new GetAllWorkshopsByOwnerIdQuery(new OwnerId(ownerId));
        var workshops = await workshopQueryService.Handle(query, cancellationToken);

        var resources = workshops.Select(WorkshopResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }
}
