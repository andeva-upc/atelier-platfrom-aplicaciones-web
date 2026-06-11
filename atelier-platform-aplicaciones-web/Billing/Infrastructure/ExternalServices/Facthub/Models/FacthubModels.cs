using System.Collections.Generic;

namespace atelier_platform_aplicaciones_web.Billing.Infrastructure.ExternalServices.Facthub.Models;

public class FacthubItem
{
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public class FacthubIssueRequest
{
    public string IssuerRuc { get; set; } = string.Empty;
    public string DocumentType { get; set; } = string.Empty;
    public string CustomerDocumentType { get; set; } = string.Empty;
    public string CustomerDocumentNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public List<FacthubItem> Items { get; set; } = new();
}

public class FacthubInvoiceResponse
{
    public string Id { get; set; } = string.Empty;
    public string Series { get; set; } = string.Empty;
    public int Number { get; set; }
    public decimal TotalAmount { get; set; }
}

public class FacthubIssueResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public FacthubInvoiceResponse? Invoice { get; set; }
}
