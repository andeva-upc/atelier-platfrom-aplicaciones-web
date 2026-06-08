using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Transform;

public static class IngestTelemetryBatchCommandFromResourceAssembler
{
    public static IngestTelemetryBatchCommand ToCommandFromResource(IngestTelemetryBatchResource resource)
    {
        var readings = resource.Readings
            .Select(r => new IngestTelemetryBatchCommand.TelemetryReading(r.Parameter, r.Value, r.Unit))
            .ToList();

        return new IngestTelemetryBatchCommand(resource.DeviceId, resource.RecordedAt, readings);
    }
}
