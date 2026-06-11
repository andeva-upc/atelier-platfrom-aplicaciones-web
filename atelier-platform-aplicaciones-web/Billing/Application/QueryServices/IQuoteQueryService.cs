using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.Aggregates;
using atelier_platform_aplicaciones_web.Billing.Domain.Model.Queries;

namespace atelier_platform_aplicaciones_web.Billing.Application.QueryServices;

public interface IQuoteQueryService
{
    Task<Quote?> Handle(GetQuoteByIdQuery query);
}
