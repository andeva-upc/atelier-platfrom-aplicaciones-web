using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;

public class DtcAlert
{
    public Guid Id { get; private set; }
    public Guid TelemetrySnapshotId { get; private set; }
    public Guid BranchId { get; private set; }
    public DtcCode DtcCode { get; private set; } = null!;
    public string Description { get; private set; } = string.Empty;
    public DtcSeverity Severity { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Required by EF Core
    protected DtcAlert() {}

    public DtcAlert(Guid telemetrySnapshotId, Guid branchId, DtcCode dtcCode, string description)
    {
        Id = Guid.NewGuid();
        TelemetrySnapshotId = telemetrySnapshotId;
        BranchId = branchId;
        DtcCode = dtcCode;
        Description = description;
        Severity = ResolveSeverity(dtcCode);
        CreatedAt = DateTime.UtcNow;
    }

    private static DtcSeverity ResolveSeverity(DtcCode code)
    {
        string val = code.Value;
        if (val.Length >= 2)
        {
            char category = val[0];
            char digit = val[1];
            if (category == 'P' && digit == '0') return DtcSeverity.CRITICAL;
            if (category == 'P') return DtcSeverity.HIGH;
            if (category == 'B' || category == 'C') return DtcSeverity.MEDIUM;
        }
        return DtcSeverity.LOW;
    }
}
