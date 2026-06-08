using atelier_platform_aplicaciones_web.Core.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Core.Interfaces.REST.Resources;

namespace atelier_platform_aplicaciones_web.Core.Interfaces.REST.Transform;

public static class CreateCustomerCommandFromResourceAssembler
{
    public static CreateCustomerCommand ToCommandFromResource(CreateCustomerResource resource)
    {
        return new CreateCustomerCommand(
            new UserId(resource.UserId),
            resource.IsCorporate,
            !resource.IsCorporate && !string.IsNullOrWhiteSpace(resource.FirstName) && !string.IsNullOrWhiteSpace(resource.LastName) 
                ? new PersonName(resource.FirstName, resource.LastName) 
                : null,
            resource.IsCorporate ? resource.BusinessName : null,
            new Document(resource.DocumentType, resource.DocumentNumber),
            new Phone(resource.Phone)
        );
    }
}
