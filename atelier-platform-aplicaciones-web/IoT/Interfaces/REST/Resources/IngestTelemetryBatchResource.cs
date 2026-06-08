namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

public record IngestTelemetryBatchResource(
    Guid DeviceId,
    DateTime RecordedAt,
    List<TelemetryReadingResource> Readings
);
