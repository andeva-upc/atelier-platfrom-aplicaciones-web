using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;

public class OBD2Device : IAuditableEntity
{
    public Guid Id { get; private set; }
    public Guid BranchId { get; private set; }
    public string MacAddress { get; private set; } = string.Empty;
    public DateTime? LastPing { get; private set; }
    public OBD2DeviceStatus Status { get; private set; }
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; private set; }
    public long Version { get; private set; }

    // Required by EF Core
    protected OBD2Device() {}

    public OBD2Device(Guid branchId, string macAddress)
    {
        Id = Guid.NewGuid();
        BranchId = branchId;
        MacAddress = macAddress;
        Status = OBD2DeviceStatus.AVAILABLE;
    }

    public void Link()
    {
        Status = OBD2DeviceStatus.LINKED;
    }

    public void Unlink()
    {
        Status = OBD2DeviceStatus.AVAILABLE;
    }

    public void RecordPing()
    {
        LastPing = DateTime.UtcNow;
    }

    public void MarkAsDeleted()
    {
        DeletedAt = DateTime.UtcNow;
    }
}
