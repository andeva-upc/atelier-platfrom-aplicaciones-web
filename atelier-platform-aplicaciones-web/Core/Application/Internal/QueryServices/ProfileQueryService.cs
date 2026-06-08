using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Core.Application.QueryServices;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.Core.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.Core.Application.Internal.QueryServices;

public class ProfileQueryService(
    ICustomerRepository customerRepository,
    IOwnerRepository ownerRepository,
    IEmployeeRepository employeeRepository) : IProfileQueryService
{
    public async Task<IEnumerable<string>> Handle(GetProfileRolesByUserIdQuery query, CancellationToken cancellationToken = default)
    {
        var roles = new List<string>();

        if (await customerRepository.ExistsByUserIdAsync(query.UserId, cancellationToken))
        {
            roles.Add("CUSTOMER");
        }
        if (await ownerRepository.ExistsByUserIdAsync(query.UserId, cancellationToken))
        {
            roles.Add("OWNER");
        }
        if (await employeeRepository.ExistsByUserIdAsync(query.UserId, cancellationToken))
        {
            roles.Add("EMPLOYEE");
        }

        return roles;
    }
}
