using atelier_platform_aplicaciones_web.Operations.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Operations.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Operations.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.Shared.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Operations.Interfaces.REST.Transform;

public static class CreateWorkOrderCommandFromResourceAssembler
{
    public static CreateWorkOrderCommand ToCommandFromResource(CreateWorkOrderResource resource)
    {
        return new CreateWorkOrderCommand(
            new AppointmentId(resource.AppointmentId),
            new BranchId(resource.BranchId),
            new VehicleId(resource.VehicleId),
            new CustomerId(resource.CustomerId),
            new DiagnosticSummary(resource.DiagnosticSummary),
            new Mileage(resource.MileageIn)
        );
    }
}