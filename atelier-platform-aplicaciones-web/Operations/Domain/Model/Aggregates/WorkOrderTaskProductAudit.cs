using System;
using atelier_platform_aplicaciones_web.Shared.Domain.Model;

namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.Aggregates;

public partial class WorkOrderTaskProduct : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    
    public DateTimeOffset? DeletedAt { get; set; }

    public long Version { get; set; }
}