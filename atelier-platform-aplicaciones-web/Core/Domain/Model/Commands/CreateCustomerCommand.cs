using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Core.Domain.Model.Commands;

public record CreateCustomerCommand(
    UserId UserId,
    bool IsCorporate,
    PersonName? Name,
    string? BusinessName,
    Document Document,
    Phone Phone
);
