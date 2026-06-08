namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

public record DtcAlertResource(
    Guid Id,
    Guid TelemetrySnapshotId,
    Guid BranchId,
    string DtcCode,
    string Description,
    string Severity,
    DateTime CreatedAt
);
