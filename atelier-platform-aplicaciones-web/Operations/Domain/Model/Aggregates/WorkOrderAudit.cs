using System;
using atelier_platform_aplicaciones_web.Shared.Domain.Model;

namespace atelier_platform_aplicaciones_web.Operations.Domain.Model.Aggregates;

public partial class WorkOrder : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    
    public DateTimeOffset? DeletedAt { get; set; }
    
    public Guid? CreatedBy { get; set; }
    
    public Guid? UpdatedBy { get; set; }

    public long Version { get; set; }
}