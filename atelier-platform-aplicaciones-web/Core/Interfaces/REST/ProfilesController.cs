using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Core.Application.QueryServices;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace atelier_platform_aplicaciones_web.Core.Interfaces.REST;

[ApiController]
[Route("api/v1/profiles")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Profiles")]
[Authorize]
public class ProfilesController(IProfileQueryService profileQueryService) : ControllerBase
{
    [HttpGet("users/{userId}/roles")]
    [SwaggerOperation(
        Summary = "Get all profile roles for a specific user ID",
        Description = "Returns a list of roles (e.g. OWNER, CUSTOMER, EMPLOYEE) that the user currently has.")]
    public async Task<ActionResult<IEnumerable<string>>> GetUserProfileRoles(Guid userId, CancellationToken cancellationToken)
    {
        var query = new GetProfileRolesByUserIdQuery(new UserId(userId));
        var roles = await profileQueryService.Handle(query, cancellationToken);
        return Ok(roles);
    }
}
