using System;
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
[Route("api/v1/obd2-devices")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("OBD2 Devices")]
[Authorize]
public class Obd2DevicesController(
    IObd2DeviceCommandService obd2DeviceCommandService,
    IStringLocalizer<IoTMessages> localizer) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Register a new OBD2 device", Description = "Registers a new physical OBD2 device in the specified branch")]
    public async Task<ActionResult> CreateObd2Device([FromBody] CreateObd2DeviceResource resource, CancellationToken cancellationToken)
    {
        var command = CreateObd2DeviceCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await obd2DeviceCommandService.Handle(command, cancellationToken);

        return ActionResultFromIoTCommandResultAssembler.ToCreatedActionResult(
            result,
            Obd2DeviceResourceFromEntityAssembler.ToResourceFromEntity,
            this,
            localizer
        );
    }

    [HttpPut("{id}")]
    [SwaggerOperation(Summary = "Update an OBD2 device details", Description = "Updates the MAC address of an existing OBD2 device")]
    public async Task<ActionResult> UpdateObd2Device(Guid id, [FromBody] UpdateObd2DeviceResource resource, CancellationToken cancellationToken)
    {
        var command = UpdateObd2DeviceCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await obd2DeviceCommandService.Handle(command, cancellationToken);

        return ActionResultFromIoTCommandResultAssembler.ToOkActionResult(
            result,
            Obd2DeviceResourceFromEntityAssembler.ToResourceFromEntity,
            this,
            localizer
        );
    }
}
