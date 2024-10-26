using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SachkovTech.Core.Models;
using SachkovTech.Framework;
using SachkovTech.Issues.Application.Features.IssueSolving.Commands.SendOnReview;
using SachkovTech.Issues.Application.Features.IssueSolving.Commands.StopWorking;
using SachkovTech.Issues.Application.Features.IssueSolving.Commands.TakeOnWork;
using SachkovTech.Issues.Application.Features.IssueSolving.Queries.GetUserIssuesByModuleWithPagination;
using SachkovTech.Issues.Contracts.Requests;
using SachkovTech.Issues.Presentation.IssueSolving.Requests;

namespace SachkovTech.Issues.Presentation.IssueSolving;

public class IssueSolvingController : ApplicationController
{
    //[Authorize]
    [HttpPost("{moduleId:guid}/{issueId:guid}")]
    public async Task<ActionResult> TakeOnWork(
        [FromRoute] Guid moduleId,
        [FromRoute] Guid issueId,
        [FromServices] TakeOnWorkHandler handler,
        CancellationToken cancellationToken = default)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == CustomClaims.Id).Value;

        var command = new TakeOnWorkCommand(Guid.Parse(userId), issueId, moduleId);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [Authorize]
    [HttpPost("{userIssueId:guid}/review")]
    public async Task<ActionResult> SendOnReview(
        [FromRoute] Guid userIssueId,
        [FromServices] SendOnReviewHandler handler,
        [FromBody] SendOnReviewRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == CustomClaims.Id).Value;

        var command = new SendOnReviewCommand(userIssueId, Guid.Parse(userId), request.PullRequestUrl);

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [Authorize]
    [HttpPost("{userIssueId:guid}/cancel")]
    public async Task<ActionResult> StopWorking(
        [FromRoute] Guid userIssueId,
        [FromServices] StopWorkingHandler handler,
        CancellationToken cancellationToken = default)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == CustomClaims.Id).Value;

        var command = new StopWorkingCommand(userIssueId, Guid.Parse(userId));

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }
    
    [HttpGet]
    public async Task<ActionResult> GetUserIssuesByModuleId(
        [FromQuery] GetUserIssuesByModuleWithPaginationRequest request,
        [FromServices] GetUserIssuesByModuleWithPaginationHandler handler,
        CancellationToken cancellationToken = default)
    {
        var query = request.ToQuery();
        var response = await handler.Handle(query, cancellationToken);
        
        return Ok(response);
    }
}