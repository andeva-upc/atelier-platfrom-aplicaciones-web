using System;

namespace atelier_platform_aplicaciones_web.Core.Interfaces.REST.Resources;

public record CustomerResource(
    Guid Id,
    Guid UserId,
    bool IsCorporate,
    string? FirstName,
    string? LastName,
    string? BusinessName,
    string DocumentType,
    string DocumentNumber,
    string Phone
);
