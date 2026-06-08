using atelier_platform_aplicaciones_web.IoT.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.IoT.Domain.Services;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/vh_dtc_errors")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Endpoints for reporting Diagnostic Trouble Code (DTC) faults and listing active warnings")]
public class VehicleDtcErrorsController : ControllerBase
{
    private readonly IDtcCommandService _dtcCommandService;
    private readonly IDtcQueryService _dtcQueryService;

    public VehicleDtcErrorsController(
        IDtcCommandService dtcCommandService,
        IDtcQueryService dtcQueryService)
    {
        _dtcCommandService = dtcCommandService;
        _dtcQueryService = dtcQueryService;
    }

    [HttpPost("report")]
    [SwaggerOperation(Summary = "Report a new DTC warning", Description = "Manually creates a DTC error linked to the latest telemetry snapshot of a device. Severity auto-calculates from prefix.")]
    [SwaggerResponse(StatusCodes.Status201Created, "Alert reported successfully", typeof(DtcAlertResource))]
    [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid code or parameter formats")]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Device active registration or latest snapshot not found")]
    public async Task<IActionResult> ReportDtcError([FromBody] ReportDtcErrorResource resource)
    {
        var command = ReportDtcErrorCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await _dtcCommandService.Handle(command);

        return result.Fold<IActionResult>(
            alert => Created(string.Empty, DtcAlertResourceFromAggregateAssembler.ToResourceFromAggregate(alert)),
            error =>
            {
                if (error.Contains("notFound") || error.Contains("noTelemetry")) return NotFound(new { error });
                return BadRequest(new { error });
            }
        );
    }

    [HttpGet("active")]
    [SwaggerOperation(Summary = "Get active DTC alerts", Description = "Retrieves a list of active DTC warning alerts within a given branch.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Active warnings list", typeof(IEnumerable<DtcAlertResource>))]
    public async Task<IActionResult> GetActiveDtcErrors([FromQuery] Guid branchId)
    {
        var query = new GetActiveDtcAlertsQuery(branchId);
        var alerts = await _dtcQueryService.Handle(query);
        var resources = alerts.Select(DtcAlertResourceFromAggregateAssembler.ToResourceFromAggregate);

        return Ok(resources);
    }
}
