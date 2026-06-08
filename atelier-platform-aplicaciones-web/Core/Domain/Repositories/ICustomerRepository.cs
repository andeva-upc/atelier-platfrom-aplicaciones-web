using System;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.Core.Domain.Repositories;

public interface ICustomerRepository : IBaseRepository<Customer>
{
    Task<Customer?> FindCustomerByIdAsync(CustomerId id, CancellationToken cancellationToken);
    Task<Customer?> FindByUserIdAsync(UserId userId, CancellationToken cancellationToken);
    Task<bool> ExistsByUserIdAsync(UserId userId, CancellationToken cancellationToken);
}
