using System;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Core.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Domain.Repositories;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using atelier_platform_aplicaciones_web.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace atelier_platform_aplicaciones_web.Core.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class EmployeeRepository(AppDbContext context) : BaseRepository<Employee>(context), IEmployeeRepository
{
    public async Task<Employee?> FindEmployeeByIdAsync(EmployeeId id, CancellationToken cancellationToken)
    {
        return await Context.Set<Employee>().FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<Employee?> FindByUserIdAsync(UserId userId, CancellationToken cancellationToken)
    {
        return await Context.Set<Employee>().FirstOrDefaultAsync(e => e.UserId == userId, cancellationToken);
    }

    public async Task<bool> ExistsByUserIdAsync(UserId userId, CancellationToken cancellationToken)
    {
        return await Context.Set<Employee>().AnyAsync(e => e.UserId == userId, cancellationToken);
    }

    // Explicit Soft Delete implementation
    void IBaseRepository<Employee>.Remove(Employee entity)
    {
        entity.DeletedAt = DateTimeOffset.UtcNow;
        Update(entity);
    }
}
