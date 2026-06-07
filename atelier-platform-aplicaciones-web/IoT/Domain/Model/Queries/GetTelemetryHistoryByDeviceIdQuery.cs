namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.Queries;

public record GetTelemetryHistoryByDeviceIdQuery(Guid DeviceId, DateTime? From, DateTime? To);
