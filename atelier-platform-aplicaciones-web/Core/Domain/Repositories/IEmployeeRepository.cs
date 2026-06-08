using System;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.Core.Domain.Repositories;

public interface IEmployeeRepository : IBaseRepository<Employee>
{
    Task<Employee?> FindEmployeeByIdAsync(EmployeeId id, CancellationToken cancellationToken);
    Task<Employee?> FindByUserIdAsync(UserId userId, CancellationToken cancellationToken);
    Task<bool> ExistsByUserIdAsync(UserId userId, CancellationToken cancellationToken);
}
