using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.IoT.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Shared.Application.Model;

namespace atelier_platform_aplicaciones_web.IoT.Application.CommandServices;

public interface IObd2DeviceCommandService
{
    Task<Result<Obd2Device>> Handle(CreateObd2DeviceCommand command, CancellationToken cancellationToken = default);
}
