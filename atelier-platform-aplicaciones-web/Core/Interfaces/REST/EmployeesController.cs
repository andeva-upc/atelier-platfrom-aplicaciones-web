using System;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using atelier_platform_aplicaciones_web.Core.Application.CommandServices;
using atelier_platform_aplicaciones_web.Core.Application.QueryServices;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Commands;
using atelier_platform_aplicaciones_web.Core.Domain.Model.Queries;
using atelier_platform_aplicaciones_web.Core.Domain.Model.ValueObjects;
using atelier_platform_aplicaciones_web.Core.Interfaces.REST.Resources;
using atelier_platform_aplicaciones_web.Core.Interfaces.REST.Transform;
using atelier_platform_aplicaciones_web.Core.Resources;
using atelier_platform_aplicaciones_web.IAM.Infrastructure.Pipeline.Middleware.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Swashbuckle.AspNetCore.Annotations;

namespace atelier_platform_aplicaciones_web.Core.Interfaces.REST;

[ApiController]
[Route("api/v1/employees")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Employees")]
[Authorize]
public class EmployeesController(
    IEmployeeCommandService employeeCommandService,
    IEmployeeQueryService employeeQueryService,
    IStringLocalizer<CoreMessages> localizer) : ControllerBase
{
    [HttpPost]
    [SwaggerOperation(Summary = "Create a new employee profile", Description = "Creates a new employee profile associated with a user ID")]
    public async Task<ActionResult> CreateEmployee([FromBody] CreateEmployeeResource resource, CancellationToken cancellationToken)
    {
        var command = CreateEmployeeCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await employeeCommandService.Handle(command, cancellationToken);

        return ActionResultFromCoreCommandResultAssembler.ToCreatedAtActionResult(
            result,
            EmployeeResourceFromEntityAssembler.ToResourceFromEntity,
            this,
            localizer,
            nameof(GetEmployeeById),
            "employeeId"
        );
    }

    [HttpPut("user/{userId}")]
    [SwaggerOperation(Summary = "Update an employee profile", Description = "Updates an existing employee profile using the user ID")]
    public async Task<ActionResult> UpdateEmployee(Guid userId, [FromBody] UpdateEmployeeResource resource, CancellationToken cancellationToken)
    {
        var command = UpdateEmployeeCommandFromResourceAssembler.ToCommandFromResource(userId, resource);
        var result = await employeeCommandService.Handle(command, cancellationToken);

        return ActionResultFromCoreCommandResultAssembler.ToOkActionResult(
            result,
            EmployeeResourceFromEntityAssembler.ToResourceFromEntity,
            this,
            localizer
        );
    }

    [HttpGet("{employeeId}")]
    [ActionName(nameof(GetEmployeeById))]
    [SwaggerOperation(Summary = "Get an employee profile by ID", Description = "Retrieves the details of a specific employee profile")]
    public async Task<ActionResult> GetEmployeeById(Guid employeeId, CancellationToken cancellationToken)
    {
        var query = new GetEmployeeByIdQuery(new EmployeeId(employeeId));
        var employee = await employeeQueryService.Handle(query, cancellationToken);

        if (employee == null) return NotFound();

        return Ok(EmployeeResourceFromEntityAssembler.ToResourceFromEntity(employee));
    }

    [HttpDelete("user/{userId}")]
    [SwaggerOperation(Summary = "Delete an employee profile", Description = "Deletes an existing employee profile using the user ID")]
    public async Task<ActionResult> DeleteEmployee(Guid userId, CancellationToken cancellationToken)
    {
        var command = new DeleteEmployeeCommand(new UserId(userId));
        var result = await employeeCommandService.Handle(command, cancellationToken);

        return ActionResultFromCoreCommandResultAssembler.ToNoContentActionResult(
            result,
            this,
            localizer
        );
    }
}
