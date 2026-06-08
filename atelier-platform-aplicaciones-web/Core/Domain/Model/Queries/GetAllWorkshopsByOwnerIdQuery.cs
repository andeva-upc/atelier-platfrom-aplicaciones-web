using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Core.Domain.Model.Queries;

public record GetAllWorkshopsByOwnerIdQuery(OwnerId OwnerId);
