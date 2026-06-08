using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Shared.Application.Model;

namespace atelier_platform_aplicaciones_web.Core.Application.CommandServices;

public interface ICustomerCommandService
{
    Task<Result<Customer>> Handle(CreateCustomerCommand command, CancellationToken cancellationToken = default);
    Task<Result<Customer>> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken = default);
    Task<Result> Handle(DeleteCustomerCommand command, CancellationToken cancellationToken = default);
}
