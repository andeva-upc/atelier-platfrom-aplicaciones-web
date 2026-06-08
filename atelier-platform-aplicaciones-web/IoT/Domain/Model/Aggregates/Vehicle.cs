using atelier_platform_aplicaciones_web.Shared.Domain.Model.Entities;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;

public partial class Vehicle : IAuditableEntity
{
    public VehicleId Id { get; private set; }
    public string PlateNumber { get; private set; } = string.Empty;
    public string Vin { get; private set; } = string.Empty;
    public int Year { get; private set; }
    public string Brand { get; private set; } = string.Empty;
    public string Model { get; private set; } = string.Empty;
    
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTimeOffset? DeletedAt { get; set; }

    // Required by EF Core
    protected Vehicle() {}

    public Vehicle(string plateNumber, string vin, int year, string brand, string model): this()
    {
        Id = new VehicleId(Guid.NewGuid());
        PlateNumber = plateNumber;
        Vin = vin;
        Year = year;
        Brand = brand;
        Model = model;
    }

    public void UpdateDetails(string plateNumber, string vin, int year, string brand, string model)
    {
        PlateNumber = plateNumber;
        Vin = vin;
        Year = year;
        Brand = brand;
        Model = model;
    }

    public void MarkAsDeleted()
    {
        DeletedAt = DateTime.UtcNow;
    }
}
