using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Core.Application.QueryServices;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.Core.Domain.Repositories;

namespace atelier_platform_aplicaciones_web.Core.Application.Internal.QueryServices;

public class CustomerQueryService(ICustomerRepository customerRepository) : ICustomerQueryService
{
    public async Task<Customer?> Handle(GetCustomerByIdQuery query, CancellationToken cancellationToken = default)
    {
        return await customerRepository.FindCustomerByIdAsync(query.Id, cancellationToken);
    }
}
