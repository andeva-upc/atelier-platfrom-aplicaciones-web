using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.IoT.Application.CommandServices;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Transform;
using atelier_platform_aplicaciones_web.IoT.Resources;
using atelier_platform_aplicaciones_web.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST;

[ApiController]
[Route("api/v1/obd2-device-registrations")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("OBD2 Device Registrations")]
[Authorize]
public class Obd2DeviceRegistrationsController(
    IObd2DeviceRegistrationCommandService registrationCommandService,
    IStringLocalizer<IoTMessages> localizer) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Link an OBD2 device to a vehicle", Description = "Creates a new active registration mapping an OBD2 device to a vehicle")]
    public async Task<ActionResult> LinkObd2Device([FromBody] CreateObd2DeviceRegistrationResource resource, CancellationToken cancellationToken)
    {
        var command = LinkObd2DeviceToVehicleCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await registrationCommandService.Handle(command, cancellationToken);

        return ActionResultFromIoTCommandResultAssembler.ToCreatedActionResult(
            result,
            Obd2DeviceRegistrationResourceFromEntityAssembler.ToResourceFromEntity,
            this,
            localizer
        );
    }
}
