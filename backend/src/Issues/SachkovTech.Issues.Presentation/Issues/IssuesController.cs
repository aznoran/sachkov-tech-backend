using Microsoft.AspNetCore.Mvc;
using SachkovTech.Framework;
using SachkovTech.Framework.Authorization;
using SachkovTech.Issues.Application.Features.Issue.Commands.AddIssue;
using SachkovTech.Issues.Application.Features.Issue.Commands.DeleteIssue;
using SachkovTech.Issues.Application.Features.Issue.Commands.DeleteIssue.SoftDeleteIssue;
using SachkovTech.Issues.Application.Features.Issue.Commands.ForceDeleteIssue;
using SachkovTech.Issues.Application.Features.Issue.Commands.RestoreIssue;
using SachkovTech.Issues.Application.Features.Issue.Commands.UpdateIssueMainInfo;
using SachkovTech.Issues.Application.Features.Issue.Queries.GetIssueById;
using SachkovTech.Issues.Application.Features.Issue.Queries.GetIssuesByModuleWithPagination;
using SachkovTech.Issues.Application.Features.Issue.Queries.GetIssuesWithPagination;
using SachkovTech.Issues.Presentation.Issues.Requests;

namespace SachkovTech.Issues.Presentation.Issues;

public class IssuesController : ApplicationController
{
    [Permission(Permissions.Issues.ReadIssue)]
    [HttpGet("dapper")]
    public async Task<ActionResult> GetDapper(
        [FromQuery] GetIssuesWithPaginationRequest request,
        [FromServices] GetIssuesWithPaginationHandlerDapper handler,
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();
        
        var response = await handler.Handle(query, cancellationToken);
        
        return Ok(response);
    }
    
    [Permission(Permissions.Issues.ReadIssue)]
    [HttpGet]
    public async Task<ActionResult> Get(
        [FromQuery] GetIssuesWithPaginationRequest request,
        [FromServices] GetIssuesWithPaginationHandler handler,
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery();
        
        var response = await handler.Handle(query, cancellationToken);
        
        return Ok(response);
    }
    
    [Permission(Permissions.Issues.ReadIssue)]
    [HttpGet("module/{moduleId:guid}")]
    public async Task<ActionResult> GetByModule(
        [FromRoute] Guid moduleId,
        [FromQuery] GetIssuesByModuleWithPaginationRequest request,
        [FromServices] GetIssuesByModuleWithPaginationHandlerDapper handlerDapper,
        CancellationToken cancellationToken)
    {
        var query = request.ToQuery(moduleId);
        
        var response = await handlerDapper.Handle(query, cancellationToken);
        
        if (response.IsFailure)
            return response.Error.ToResponse();
        
        return Ok(response.Value);
    }

    [Permission(Permissions.Issues.ReadIssue)]
    [HttpGet("{id:guid}")]
    public async Task<ActionResult> GetById(
        [FromRoute] Guid id,
        [FromServices] GetIssueByIdHandler handler,
        CancellationToken cancellationToken)
    {
        var query = new GetIssueByIdQuery(id);

        var response = await handler.Handle(query, cancellationToken);

        if (response.IsFailure)
            return response.Error.ToResponse();

        return Ok(response.Value);
    }
    
    [Permission(Permissions.Issues.CreateIssue)]
    [HttpPost]
    public async Task<ActionResult> AddIssue(
        [FromBody] AddIssueRequest request,
        [FromServices] AddIssueHandler handler,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand();

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [Permission(Permissions.Issues.UpdateIssue)]
    [HttpPut("{issueId:guid}/main-info")]
    public async Task<ActionResult> UpdateIssueMainInfo(
        [FromRoute] Guid issueId,
        [FromBody] UpdateIssueMainInfoRequest request,
        [FromServices] UpdateIssueMainInfoHandler handler,
        CancellationToken cancellationToken)
    {
        var command = request.ToCommand(issueId);
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [Permission(Permissions.Issues.UpdateIssue)]
    [HttpPut("{issueId:guid}/restore")]
    public async Task<ActionResult> RestoreIssue(
        [FromRoute] Guid issueId,
        [FromServices] RestoreIssueHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new RestoreIssueCommand(issueId);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [Permission(Permissions.Issues.DeleteIssue)]
    [HttpDelete("{issueId:guid}/soft")]
    public async Task<ActionResult> SoftDeleteIssue(
        [FromRoute] Guid issueId,
        [FromServices] SoftDeleteIssueHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteIssueCommand(issueId);
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [Permission(Permissions.Issues.DeleteIssue)]
    [HttpDelete("{issueId:guid}/force")]
    public async Task<ActionResult> ForceDeleteIssue(
        [FromRoute] Guid issueId,
        [FromServices] ForceDeleteIssueHandler handler,
        CancellationToken cancellationToken)
    {
        var command = new DeleteIssueCommand(issueId);
        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}