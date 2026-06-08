using atelier_platform_aplicaciones_web.IoT.Domain.Services;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/vh_devices")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Endpoints for binding/linking OBD2 devices to vehicles")]
public class VehicleDevicesController : ControllerBase
{
    private readonly IOBD2DeviceCommandService _deviceCommandService;

    public VehicleDevicesController(IOBD2DeviceCommandService deviceCommandService)
    {
        _deviceCommandService = deviceCommandService;
    }

    [HttpPost("link")]
    [SwaggerOperation(Summary = "Link OBD2 device to vehicle", Description = "Binds a physical OBD2 device to a vehicle in the active branch. Responds 409 Conflict if linked.")]
    [SwaggerResponse(StatusCodes.Status201Created, "Successfully linked", typeof(OBD2DeviceRegistrationResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Bad request validation")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Device or vehicle not found")]
    [SwaggerResponse(StatusCodes.Status409Conflict, "Device or vehicle already linked")]
    [SwaggerResponse(StatusCodes.Status422UnprocessableEntity, "Unprocessable request payload")]
    public async Task<IActionResult> LinkDevice([FromBody] LinkDeviceResource resource)
    {
        var command = LinkDeviceCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await _deviceCommandService.Handle(command);

        return result.Fold<IActionResult>(
            registration => Created(string.Empty, OBD2DeviceRegistrationResourceFromAggregateAssembler.ToResourceFromAggregate(registration)),
            error =>
            {
                if (error.Contains("notFound")) return NotFound(new { error });
                if (error.Contains("already") || error.Contains("HasDevice")) return Conflict(new { error });
                return UnprocessableEntity(new { error });
            }
        );
    }

    [HttpPost("unlink")]
    [SwaggerOperation(Summary = "Unlink OBD2 device from vehicle", Description = "Deactivates the active registration of the OBD2 device and makes it available again.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Successfully unlinked", typeof(OBD2DeviceRegistrationResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Active registration not found")]
    public async Task<IActionResult> UnlinkDevice([FromBody] UnlinkDeviceResource resource)
    {
        var command = new Domain.Model.Commands.UnlinkDeviceCommand(resource.Obd2DeviceId);
        var result = await _deviceCommandService.Handle(command);

        return result.Fold<IActionResult>(
            registration => Ok(OBD2DeviceRegistrationResourceFromAggregateAssembler.ToResourceFromAggregate(registration)),
            error => NotFound(new { error })
        );
    }
}
