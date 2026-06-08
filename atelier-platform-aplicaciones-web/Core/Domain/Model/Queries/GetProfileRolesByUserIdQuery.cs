using System;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;

namespace atelier_platform_aplicaciones_web.Core.Domain.Model.Queries;

public record GetProfileRolesByUserIdQuery
{
    public GetProfileRolesByUserIdQuery(UserId userId)
    {
        if (userId == null)
        {
            throw new ArgumentNullException(nameof(userId), "userId cannot be null");
        }
        UserId = userId;
    }

    public UserId UserId { get; init; }
}
