namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;

public record IngestTelemetryBatchCommand(
    Guid DeviceId,
    DateTime RecordedAt,
    List<IngestTelemetryBatchCommand.TelemetryReading> Readings
)
{
    public record TelemetryReading(
        string Parameter,
        double Value,
        string Unit
    );
}
