using atelier_platform_aplicaciones_web.IoT.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.IoT.Domain.Services;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Transform;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/vehicles")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Endpoints for vehicle owner registration and branch listings")]
public class VehiclesController : ControllerBase
{
    private readonly IVehicleCommandService _vehicleCommandService;
    private readonly IVehicleQueryService _vehicleQueryService;

    public VehiclesController(
        IVehicleCommandService vehicleCommandService,
        IVehicleQueryService vehicleQueryService)
    {
        _vehicleCommandService = vehicleCommandService;
        _vehicleQueryService = vehicleQueryService;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Register a vehicle to a customer", Description = "Registers a vehicle. If vehicle already exists by VIN, its properties are updated, previous active owners are deactivated, and associated active OBD2 devices are automatically unlinked.")]
    [SwaggerResponse(StatusCodes.Status201Created, "Vehicle registered successfully", typeof(VehicleResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid data or validation error")]
    public async Task<IActionResult> RegisterVehicle([FromBody] RegisterVehicleResource resource)
    {
        var command = RegisterVehicleCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await _vehicleCommandService.Handle(command);

        return result.Fold<IActionResult>(
            vehicle => Created(string.Empty, VehicleResourceFromAggregateAssembler.ToResourceFromAggregate(vehicle)),
            error => BadRequest(new { message = error })
        );
    }

    [HttpGet("branch/{branchId:guid}")]
    [SwaggerOperation(Summary = "List vehicles in branch", Description = "Retrieves a list of vehicles actively owned by customers registered to the branch.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Active vehicles in branch", typeof(IEnumerable<VehicleResource>))]
    public async Task<IActionResult> GetVehiclesInBranch(Guid branchId)
    {
        var query = new GetVehiclesByBranchIdQuery(new BranchId(branchId));
        var vehicles = await _vehicleQueryService.Handle(query);
        var resources = vehicles.Select(VehicleResourceFromAggregateAssembler.ToResourceFromAggregate);

        return Ok(resources);
    }
}
