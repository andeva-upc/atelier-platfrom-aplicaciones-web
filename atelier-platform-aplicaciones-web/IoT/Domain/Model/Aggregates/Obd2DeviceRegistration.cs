using System;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;

public class Obd2DeviceRegistration
{
    public Obd2DeviceRegistrationId Id { get; private set; }
    public Obd2DeviceId Obd2DeviceId { get; private set; }
    public BranchId BranchId { get; private set; }
    public VehicleId VehicleId { get; private set; }
    public Obd2RegistrationStatus Status { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public DateTimeOffset? DeletedAt { get; private set; }

    private Obd2DeviceRegistration()
    {
        Id = null!;
        Obd2DeviceId = null!;
        BranchId = null!;
        VehicleId = null!;
        Status = null!;
    }

    public Obd2DeviceRegistration(Obd2DeviceId obd2DeviceId, BranchId branchId, VehicleId vehicleId)
    {
        Id = new Obd2DeviceRegistrationId();
        Obd2DeviceId = obd2DeviceId;
        BranchId = branchId;
        VehicleId = vehicleId;
        Status = Obd2RegistrationStatus.Active;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public void Deactivate()
    {
        if (Status == Obd2RegistrationStatus.Inactive)
        {
            throw new InvalidOperationException("iot.error.obd2DeviceRegistration.alreadyInactive");
        }
        Status = Obd2RegistrationStatus.Inactive;
        DeletedAt = DateTimeOffset.UtcNow;
    }
}
