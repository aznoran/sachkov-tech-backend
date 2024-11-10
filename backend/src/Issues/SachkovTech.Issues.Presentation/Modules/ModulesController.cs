using Microsoft.AspNetCore.Mvc;
using SachkovTech.Core.Dtos;
using SachkovTech.Framework;
using SachkovTech.Framework.Authorization;
using SachkovTech.Issues.Application.Features.Modules.Commands.Create;
using SachkovTech.Issues.Application.Features.Modules.Commands.Delete;
using SachkovTech.Issues.Application.Features.Modules.Commands.UpdateIssuePosition;
using SachkovTech.Issues.Application.Features.Modules.Commands.UpdateMainInfo;
using SachkovTech.Issues.Presentation.Modules.Requests;

namespace SachkovTech.Issues.Presentation.Modules;

public class ModulesController : ApplicationController
{
    [HttpGet]
    public ActionResult Get()
    {
        List<ModuleDto> response =
        [
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Module 1",
                Description = "Description 1",
                IssuesPosition = []
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Module 2",
                Description = "Description 2",
                IssuesPosition = []
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Module 3",
                Description = "Description 3",
                IssuesPosition = []
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Module 4",
                Description = "Description 4",
                IssuesPosition = []
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Module 5",
                Description = "Description 5",
                IssuesPosition = []
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Module 6",
                Description = "Description 6",
                IssuesPosition = []
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Module 7",
                Description = "Description 7",
                IssuesPosition = []
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Module 8",
                Description = "Description 8",
                IssuesPosition = []
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Module 9",
                Description = "Description 9",
                IssuesPosition = []
            },
            new()
            {
                Id = Guid.NewGuid(),
                Title = "Module 10",
                Description = "Description 10",
                IssuesPosition = []
            }
        ];

        return Ok(response);
    }

    [Permission(Permissions.Modules.CreateModule)]
    [HttpPost]
    public async Task<ActionResult> Create(
        [FromServices] CreateModuleHandler handler,
        [FromBody] CreateModuleRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [Permission(Permissions.Modules.UpdateModule)]
    [HttpPut("{id:guid}/main-info")]
    public async Task<ActionResult> UpdateMainInfo(
        [FromRoute] Guid id,
        [FromBody] UpdateMainInfoRequest request,
        [FromServices] UpdateMainInfoHandler handler,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
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
    
    [Permission(Permissions.Modules.DeleteModule)]
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