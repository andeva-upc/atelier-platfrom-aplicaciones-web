using atelier_platform_aplicaciones_web.IoT.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;

public class VehicleRegistration
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public VehicleId VehicleId { get; private set; }
    public VehicleRegistrationStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    // Required by EF Core
    protected VehicleRegistration() {}

    public VehicleRegistration(Guid userId, VehicleId vehicleId)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        VehicleId = vehicleId;
        Status = VehicleRegistrationStatus.ACTIVE;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkAsPrevious()
    {
        Status = VehicleRegistrationStatus.PREVIOUS;
        DeletedAt = DateTime.UtcNow;
    }
}
