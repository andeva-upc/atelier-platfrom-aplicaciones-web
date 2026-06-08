using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Core.Domain.Model.Commands;

public record CreateOwnerCommand(
    UserId UserId,
    PersonName Name,
    Document Document,
    Phone Phone
);
