using atelier_platform_aplicaciones_web.IoT.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.IoT.Domain.Services;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/obd2-devices")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Endpoints for managing OBD2 hardware devices")]
public class OBD2DevicesController : ControllerBase
{
    private readonly IOBD2DeviceCommandService _deviceCommandService;
    private readonly IOBD2DeviceQueryService _deviceQueryService;

    public OBD2DevicesController(
        IOBD2DeviceCommandService deviceCommandService,
        IOBD2DeviceQueryService deviceQueryService)
    {
        _deviceCommandService = deviceCommandService;
        _deviceQueryService = deviceQueryService;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Register a new OBD2 device", Description = "Creates a new OBD2 device linked to a specific branch in the system.")]
    [SwaggerResponse(StatusCodes.Status201Created, "The OBD2 device was successfully created", typeof(OBD2DeviceResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request or MAC address already exists")]
    public async Task<IActionResult> CreateDevice([FromBody] CreateOBD2DeviceResource resource)
    {
        var command = CreateOBD2DeviceCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await _deviceCommandService.Handle(command);

        return result.Fold<IActionResult>(
            device => CreatedAtAction(nameof(GetDeviceById), new { id = device.Id }, 
                OBD2DeviceResourceFromAggregateAssembler.ToResourceFromAggregate(device)),
            error => BadRequest(new { message = error })
        );
    }

    [HttpGet("{id:guid}")]
    [SwaggerOperation(Summary = "Get OBD2 device by ID", Description = "Retrieves the details of a registered OBD2 device.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Device found", typeof(OBD2DeviceResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Device not found")]
    public async Task<IActionResult> GetDeviceById(Guid id)
    {
        var query = new GetOBD2DeviceByIdQuery(id);
        var device = await _deviceQueryService.Handle(query);

        if (device == null)
        {
            return NotFound();
        }

        return Ok(OBD2DeviceResourceFromAggregateAssembler.ToResourceFromAggregate(device));
    }

    [HttpGet("branch/{branchId:guid}")]
    [SwaggerOperation(Summary = "Get OBD2 devices by branch ID", Description = "Retrieves all OBD2 devices associated with a given workshop branch.")]
    [SwaggerResponse(StatusCodes.Status200OK, "List of devices", typeof(IEnumerable<OBD2DeviceResource>))]
    public async Task<IActionResult> GetDevicesByBranch(Guid branchId)
    {
        var query = new GetOBD2DevicesByBranchIdQuery(branchId);
        var devices = await _deviceQueryService.Handle(query);
        var resources = devices.Select(OBD2DeviceResourceFromAggregateAssembler.ToResourceFromAggregate);

        return Ok(resources);
    }

    [HttpPut("{id:guid}")]
    [SwaggerOperation(Summary = "Update OBD2 device details", Description = "Updates the MAC address and branch mapping for an OBD2 device.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Device updated", typeof(OBD2DeviceResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid request or MAC address collision")]
    public async Task<IActionResult> UpdateDevice(Guid id, [FromBody] CreateOBD2DeviceResource resource)
    {
        var command = new Domain.Model.Commands.UpdateOBD2DeviceCommand(id, resource.BranchId, resource.MacAddress);
        var result = await _deviceCommandService.Handle(command);

        return result.Fold<IActionResult>(
            device => Ok(OBD2DeviceResourceFromAggregateAssembler.ToResourceFromAggregate(device)),
            error => BadRequest(new { message = error })
        );
    }

    [HttpDelete("{id:guid}")]
    [SwaggerOperation(Summary = "Delete OBD2 device", Description = "Soft-deletes the OBD2 device from the system.")]
    [SwaggerResponse(StatusCodes.Status204NoContent, "Device deleted")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Device not found")]
    public async Task<IActionResult> DeleteDevice(Guid id)
    {
        var command = new Domain.Model.Commands.DeleteOBD2DeviceCommand(id);
        var result = await _deviceCommandService.Handle(command);

        return result.Fold<IActionResult>(
            success => NoContent(),
            error => NotFound(new { message = error })
        );
    }
}
