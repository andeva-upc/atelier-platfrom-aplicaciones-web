using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Core.Domain.Model.Commands;

public record UpdateCustomerCommand(
    UserId UserId,
    PersonName? Name,
    string? BusinessName,
    Document Document,
    Phone Phone
);
