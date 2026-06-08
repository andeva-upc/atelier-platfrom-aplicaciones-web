namespace atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;

public record RegisterVehicleCommand(
    string PlateNumber,
    string Vin,
    int Year,
    string Brand,
    string Model,
    Guid UserId
);
