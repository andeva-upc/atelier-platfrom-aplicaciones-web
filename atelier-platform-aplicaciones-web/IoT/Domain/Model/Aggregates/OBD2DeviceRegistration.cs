using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;

public class OBD2DeviceRegistration
{
    public Guid Id { get; private set; }
    public Guid Obd2DeviceId { get; private set; }
    public BranchId BranchId { get; private set; }
    public VehicleId VehicleId { get; private set; }
    public OBD2DeviceRegistrationStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    // Required by EF Core
    protected OBD2DeviceRegistration() {}

    public OBD2DeviceRegistration(Guid obd2DeviceId, BranchId branchId, VehicleId vehicleId)
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
