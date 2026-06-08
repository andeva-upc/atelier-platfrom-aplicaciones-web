using System;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Core.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.Core.Interfaces.REST.Transform;

public static class UpdateCustomerCommandFromResourceAssembler
{
    public static UpdateCustomerCommand ToCommandFromResource(Guid userId, UpdateCustomerResource resource)
    {
        return new UpdateCustomerCommand(
            new UserId(userId),
            !string.IsNullOrWhiteSpace(resource.FirstName) && !string.IsNullOrWhiteSpace(resource.LastName)
                ? new PersonName(resource.FirstName, resource.LastName)
                : null,
            resource.BusinessName,
            new Document(resource.DocumentType, resource.DocumentNumber),
            new Phone(resource.Phone)
        );
    }
}
