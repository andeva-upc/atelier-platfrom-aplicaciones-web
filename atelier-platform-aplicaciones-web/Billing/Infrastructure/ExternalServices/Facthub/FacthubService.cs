using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Billing.Application.OutboundServices;
using atelier_platform_aplicaciones_web.Billing.Infrastructure.ExternalServices.Facthub.Models;

namespace atelier_platform_aplicaciones_web.Billing.Infrastructure.ExternalServices.Facthub;

public class FacthubService : IFacthubService
{
    private readonly HttpClient _httpClient;

    public FacthubService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<FacthubIssueResponse?> IssueInvoiceAsync(FacthubIssueRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync("api/invoices/issue", request);
        
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            throw new Exception($"Facthub service error: {response.StatusCode} - {errorContent}");
        }

        return await response.Content.ReadFromJsonAsync<FacthubIssueResponse>();
    }
}
