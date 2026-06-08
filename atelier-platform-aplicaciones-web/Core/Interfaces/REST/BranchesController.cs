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
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace atelier_platform_aplicaciones_web.Core.Interfaces.REST;

[ApiController]
[Route("api/v1/branches")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Branches")]
[Authorize]
public class BranchesController(
    IBranchCommandService branchCommandService,
    IBranchQueryService branchQueryService,
    ISubscriptionCommandService subscriptionCommandService,
    IStringLocalizer<CoreMessages> localizer) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new branch", Description = "Creates a new branch associated with a specific workshop")]
    public async Task<ActionResult> CreateBranch([FromBody] CreateBranchResource resource, CancellationToken cancellationToken)
    {
        var command = CreateBranchCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await branchCommandService.Handle(command, cancellationToken);

        return ActionResultFromCoreCommandResultAssembler.ToCreatedAtActionResult(
            result,
            BranchResourceFromEntityAssembler.ToResourceFromEntity,
            this,
            localizer,
            nameof(GetBranchById),
            "branchId"
        );
    }

    [HttpPut("{branchId}")]
    [SwaggerOperation(Summary = "Update an existing branch", Description = "Updates the details of a branch by its ID")]
    public async Task<ActionResult> UpdateBranch(Guid branchId, [FromBody] UpdateBranchResource resource, CancellationToken cancellationToken)
    {
        var command = UpdateBranchCommandFromResourceAssembler.ToCommandFromResource(branchId, resource);
        var result = await branchCommandService.Handle(command, cancellationToken);

        return ActionResultFromCoreCommandResultAssembler.ToOkActionResult(
            result,
            BranchResourceFromEntityAssembler.ToResourceFromEntity,
            this,
            localizer
        );
    }

    [HttpGet("{branchId}")]
    [ActionName(nameof(GetBranchById))]
    [SwaggerOperation(Summary = "Get a branch by ID", Description = "Retrieves the details of a specific branch")]
    public async Task<ActionResult> GetBranchById(Guid branchId, CancellationToken cancellationToken)
    {
        var query = new GetBranchByIdQuery(new BranchId(branchId));
        var branch = await branchQueryService.Handle(query, cancellationToken);

        if (branch == null) return NotFound();

        return Ok(BranchResourceFromEntityAssembler.ToResourceFromEntity(branch));
    }

    [HttpGet("workshop/{workshopId}")]
    [SwaggerOperation(Summary = "Get branches by workshop ID", Description = "Retrieves all branches belonging to a specific workshop")]
    public async Task<ActionResult> GetBranchesByWorkshopId(Guid workshopId, CancellationToken cancellationToken)
    {
        var query = new GetAllBranchesByWorkshopIdQuery(new WorkshopId(workshopId));
        var branches = await branchQueryService.Handle(query, cancellationToken);

        var resources = branches.Select(BranchResourceFromEntityAssembler.ToResourceFromEntity);
        return Ok(resources);
    }

    [HttpPost("{branchId}/subscriptions/pay")]
    [SwaggerOperation(Summary = "Simulate payment and assign subscription", Description = "Simulates a payment (Mock Stripe) using a dummy credit card and assigns the subscription plan")]
    public async Task<ActionResult> AssignSubscription(Guid branchId, [FromBody] AssignSubscriptionResource resource, CancellationToken cancellationToken)
    {
        var command = AssignSubscriptionCommandFromResourceAssembler.ToCommandFromResource(branchId, resource);
        var result = await subscriptionCommandService.Handle(command, cancellationToken);

        return ActionResultFromCoreCommandResultAssembler.ToCreatedActionResult(
            result,
            BranchSubscriptionResourceFromEntityAssembler.ToResourceFromEntity,
            this,
            localizer
        );
    }

    [HttpDelete("{branchId}/subscriptions/active")]
    [SwaggerOperation(Summary = "Cancel an active subscription", Description = "Cancels the currently active subscription of a branch")]
    public async Task<ActionResult> CancelSubscription(Guid branchId, CancellationToken cancellationToken)
    {
        var command = new CancelSubscriptionCommand(new BranchId(branchId));
        var result = await subscriptionCommandService.Handle(command, cancellationToken);

        return ActionResultFromCoreCommandResultAssembler.ToOkActionResult(
            result,
            BranchSubscriptionResourceFromEntityAssembler.ToResourceFromEntity,
            this,
            localizer
        );
    }
}
