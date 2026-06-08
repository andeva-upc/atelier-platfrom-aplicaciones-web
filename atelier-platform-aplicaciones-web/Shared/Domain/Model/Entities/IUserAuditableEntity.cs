namespace atelier_platform_aplicaciones_web.Shared.Domain.Model.Entities;

/// <summary>
///     Marks an entity as carrying audit timestamps and user identifiers managed by the persistence layer.
/// </summary>
/// <remarks>
///     Extends <see cref="IAuditableEntity"/> to automatically track the user who created 
///     and last updated the entity via <c>AuditableEntityInterceptor</c>.
/// </remarks>
public interface IUserAuditableEntity : IAuditableEntity
{
    /// <summary>
    ///     Gets or sets the unique identifier of the user who first persisted the entity.
    /// </summary>
    Guid? CreatedBy { get; set; }

    /// <summary>
    ///     Gets or sets the unique identifier of the user who last saved the entity.
    /// </summary>
    Guid? UpdatedBy { get; set; }
}
