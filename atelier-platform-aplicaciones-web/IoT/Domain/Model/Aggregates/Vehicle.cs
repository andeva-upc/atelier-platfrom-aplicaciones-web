using atelier_platform_aplicaciones_web.Shared.Domain.Model;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.Entities;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;

public class Vehicle : IAuditableEntity
{
    public Guid Id { get; private set; }
    public string PlateNumber { get; private set; } = string.Empty;
    public string Vin { get; private set; } = string.Empty;
    public int Year { get; private set; }
    public string Brand { get; private set; } = string.Empty;
    public string Model { get; private set; } = string.Empty;
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; private set; }
    public long Version { get; private set; }

    // Required by EF Core
    protected Vehicle() {}

    public Vehicle(string plateNumber, string vin, int year, string brand, string model)
    {
        Id = Guid.NewGuid();
        PlateNumber = plateNumber;
        Vin = vin;
        Year = year;
        Brand = brand;
        Model = model;
    }

    public void UpdateDetails(string plateNumber, int year, string brand, string model)
    {
        PlateNumber = plateNumber;
        Year = year;
        Brand = brand;
        Model = model;
    }

    public void MarkAsDeleted()
    {
        DeletedAt = DateTime.UtcNow;
    }
}
