using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Shared.Application.Model;

namespace atelier_platform_aplicaciones_web.IoT.Domain.Services;

public interface IVehicleCommandService
{
    Task<Result<Vehicle>> Handle(RegisterVehicleCommand command);
}
