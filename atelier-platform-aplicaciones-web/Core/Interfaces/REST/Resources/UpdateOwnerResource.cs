namespace atelier_platform_aplicaciones_web.Core.Interfaces.REST.Resources;

public record UpdateOwnerResource(
    string FirstName,
    string LastName,
    string DocumentType,
    string DocumentNumber,
    string Phone
);
