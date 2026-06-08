using atelier_platform_aplicaciones_web.IoT.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.IoT.Domain.Services;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Controllers;

[ApiController]
[Route("api/v1/vh_telemetry_batches")]
[Produces(MediaTypeNames.Application.Json)]
[SwaggerTag("Endpoints for device telemetry batch ingestion and historic queries")]
public class VehicleTelemetryBatchesController : ControllerBase
{
    private readonly ITelemetryCommandService _telemetryCommandService;
    private readonly ITelemetryQueryService _telemetryQueryService;

    public VehicleTelemetryBatchesController(
        ITelemetryCommandService telemetryCommandService,
        ITelemetryQueryService telemetryQueryService)
    {
        _telemetryCommandService = telemetryCommandService;
        _telemetryQueryService = telemetryQueryService;
    }

    [HttpPost]
    [SwaggerOperation(Summary = "Ingest a telemetry batch from device", Description = "Receives batch readings from physical device. Requires X-Device-Token header.")]
    [SwaggerResponse(StatusCodes.Status202Accepted, "Telemetry batch enqueued successfully")]
    [SwaggerResponse(StatusCodes.Status401Unauthorized, "Token header missing or invalid")]
    public async Task<IActionResult> IngestTelemetry(
        [FromHeader(Name = "X-Device-Token")] string? deviceToken, 
        [FromBody] IngestTelemetryBatchResource resource)
    {
        if (string.IsNullOrWhiteSpace(deviceToken))
        {
            return Unauthorized(new { message = "X-Device-Token header is missing or invalid." });
        }

        var command = IngestTelemetryBatchCommandFromResourceAssembler.ToCommandFromResource(resource);
        await _telemetryCommandService.Handle(command);

        return Accepted(new
        {
            status = "accepted",
            message = "Telemetry batch received and queued for processing",
            deviceId = resource.DeviceId
        });
    }

    [HttpGet("latest/{deviceId:guid}")]
    [SwaggerOperation(Summary = "Get the latest telemetry snapshot", Description = "Retrieves the most recent telemetry capture from the device active linkage.")]
    [SwaggerResponse(StatusCodes.Status200OK, "Latest snapshot", typeof(TelemetrySnapshotResource))]
    [SwaggerResponse(StatusCodes.Status404NotFound, "Snapshot not found")]
    public async Task<IActionResult> GetLatestTelemetry(Guid deviceId)
    {
        var query = new GetLatestTelemetryByDeviceIdQuery(deviceId);
        var snapshot = await _telemetryQueryService.Handle(query);

        if (snapshot == null)
        {
            return NotFound();
        }

        return Ok(TelemetrySnapshotResourceFromAggregateAssembler.ToResourceFromAggregate(snapshot));
    }

    [HttpGet("history/{deviceId:guid}")]
    [SwaggerOperation(Summary = "Get telemetry history for a device", Description = "Retrieves the telemetry history list with optional date range filters.")]
    [SwaggerResponse(StatusCodes.Status200OK, "History snapshots", typeof(IEnumerable<TelemetrySnapshotResource>))]
    public async Task<IActionResult> GetTelemetryHistory(
        Guid deviceId,
        [FromQuery] DateTime? from,
        [FromQuery] DateTime? to)
    {
        var query = new GetTelemetryHistoryByDeviceIdQuery(deviceId, from, to);
        var snapshots = await _telemetryQueryService.Handle(query);
        var resources = snapshots.Select(TelemetrySnapshotResourceFromAggregateAssembler.ToResourceFromAggregate);

        return Ok(resources);
    }
}
