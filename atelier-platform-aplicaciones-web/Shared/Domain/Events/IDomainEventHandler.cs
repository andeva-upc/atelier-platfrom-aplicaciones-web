using System.Threading;
using System.Threading.Tasks;
namespace atelier_platform_aplicaciones_web.Shared.Domain.Events;
public interface IDomainEventHandler<in TEvent>
{
    Task HandleAsync(TEvent domainEvent, CancellationToken cancellationToken = default);
}