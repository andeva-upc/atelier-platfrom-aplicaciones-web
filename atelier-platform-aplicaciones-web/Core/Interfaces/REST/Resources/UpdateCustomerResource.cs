namespace atelier_platform_aplicaciones_web.Core.Interfaces.REST.Resources;

public record UpdateCustomerResource(
    string? FirstName,
    string? LastName,
    string? BusinessName,
    string DocumentType,
    string DocumentNumber,
    string Phone
);
