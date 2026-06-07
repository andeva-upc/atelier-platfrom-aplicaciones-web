using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;

public class OBD2DeviceRegistration
{
    public Guid Id { get; private set; }
    public Guid Obd2DeviceId { get; private set; }
    public Guid BranchId { get; private set; }
    public Guid VehicleId { get; private set; }
    public OBD2DeviceRegistrationStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    // Required by EF Core
    protected OBD2DeviceRegistration() {}

    public OBD2DeviceRegistration(Guid obd2DeviceId, Guid branchId, Guid vehicleId)
    {
        Id = Guid.NewGuid();
        Obd2DeviceId = obd2DeviceId;
        BranchId = branchId;
        VehicleId = vehicleId;
        Status = OBD2DeviceRegistrationStatus.ACTIVE;
        CreatedAt = DateTime.UtcNow;
    }

    public void Deactivate()
    {
        Status = OBD2DeviceRegistrationStatus.INACTIVE;
        DeletedAt = DateTime.UtcNow;
    }
}
