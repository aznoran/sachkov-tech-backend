using Microsoft.AspNetCore.Mvc;
using SachkovTech.Framework;
using SachkovTech.Framework.Authorization;
using SachkovTech.Issues.Application.Features.Modules.Commands.Create;
using SachkovTech.Issues.Application.Features.Modules.Commands.Delete;
using SachkovTech.Issues.Application.Features.Modules.Commands.UpdateIssuePosition;
using SachkovTech.Issues.Application.Features.Modules.Commands.UpdateMainInfo;
using SachkovTech.Issues.Application.Features.Modules.Queries.GetModulesWithPagination;
using SachkovTech.Issues.Contracts.Requests.Module;

namespace SachkovTech.Issues.Presentation.Modules;

public class ModulesController : ApplicationController
{
    [HttpGet]
    public async Task<IActionResult> Get(
        [FromQuery] GetModulesWithPaginationQuery query,
        [FromServices] GetModulesWithPaginationHandler handler,
        CancellationToken cancellationToken)
    {
        var response = await handler.Handle(query, cancellationToken);

        return Ok(response);
    }

    [Permission(Permissions.Modules.CreateModule)]
    [HttpPost]
    public async Task<ActionResult> Create(
        [FromServices] CreateModuleHandler handler,
        [FromBody] CreateModuleRequest request,
        CancellationToken cancellationToken)
    {
        var command = new CreateModuleCommand(
            request.Title,
            request.Description);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [Permission(Permissions.Modules.UpdateModule)]
    [HttpPut("{moduleId:guid}/main-info")]
    public async Task<ActionResult> UpdateMainInfo(
        [FromRoute] Guid moduleId,
        [FromBody] UpdateMainInfoRequest request,
        [FromServices] UpdateMainInfoHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateMainInfoCommand(
            moduleId,
            request.Title,
            request.Description);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [Permission(Permissions.Issues.UpdateIssue)]
    [HttpPut("{id:guid}/issue/{issueId:guid}")]
    public async Task<ActionResult> UpdateIssuePosition(
        [FromRoute] Guid id,
        [FromRoute] Guid issueId,
        [FromBody] int newPosition,
        [FromServices] UpdateIssuePositionHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateIssuePositionCommand(id, issueId, newPosition);
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    //[Permission(Permissions.Modules.DeleteModule)]
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(
        [FromRoute] Guid id,
        [FromServices] DeleteModuleHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteModuleCommand(id);
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}