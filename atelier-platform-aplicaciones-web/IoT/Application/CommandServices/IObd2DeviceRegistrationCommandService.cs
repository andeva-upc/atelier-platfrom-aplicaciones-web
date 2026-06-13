using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Shared.Application.Model;

namespace atelier_platform_aplicaciones_web.IoT.Application.CommandServices;

public interface IObd2DeviceRegistrationCommandService
{
    Task<Result<Obd2DeviceRegistration>> Handle(LinkObd2DeviceToVehicleCommand command, CancellationToken cancellationToken = default);
}
