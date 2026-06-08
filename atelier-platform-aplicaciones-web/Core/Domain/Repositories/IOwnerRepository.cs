using System;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.Core.Domain.Repositories;

public interface IOwnerRepository : IBaseRepository<Owner>
{
    Task<Owner?> FindOwnerByIdAsync(OwnerId id, CancellationToken cancellationToken);
    Task<Owner?> FindByUserIdAsync(UserId userId, CancellationToken cancellationToken);
    Task<bool> ExistsByIdAsync(OwnerId id, CancellationToken cancellationToken);
    Task<bool> ExistsByUserIdAsync(UserId userId, CancellationToken cancellationToken);
}
