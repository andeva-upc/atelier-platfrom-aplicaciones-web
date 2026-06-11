using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Billing.Infrastructure.ExternalServices.Facthub.Models;

namespace atelier_platform_aplicaciones_web.Billing.Application.OutboundServices;

public interface IFacthubService
{
    Task<FacthubIssueResponse?> IssueInvoiceAsync(FacthubIssueRequest request);
}
