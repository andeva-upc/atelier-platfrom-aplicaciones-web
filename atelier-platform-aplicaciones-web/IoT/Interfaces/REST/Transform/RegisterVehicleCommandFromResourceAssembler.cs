using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.IoT.Interfaces.REST.Transform;

public static class RegisterVehicleCommandFromResourceAssembler
{
    public static RegisterVehicleCommand ToCommandFromResource(RegisterVehicleResource resource)
    {
        return new RegisterVehicleCommand(
            resource.PlateNumber,
            resource.Vin,
            resource.Year,
            resource.Brand,
            resource.Model,
            resource.UserId
        );
    }
}
