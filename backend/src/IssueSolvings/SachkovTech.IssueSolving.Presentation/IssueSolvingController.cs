using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SachkovTech.Core.Models;
using SachkovTech.Framework;
using SachkovTech.IssueSolving.Application.Commands.SendForRevision;
using SachkovTech.IssueSolving.Application.Commands.SendOnReview;
using SachkovTech.IssueSolving.Application.Commands.StopWorking;
using SachkovTech.IssueSolving.Application.Commands.TakeOnWork;
using SachkovTech.IssueSolving.Contracts.Requests;

namespace SachkovTech.IssueSolving.Presentation;

public class IssueSolvingController : ApplicationController
{
    //[Authorize]
    [HttpPost("{issueId:guid}")]
    public async Task<ActionResult> TakeOnWork(
        [FromRoute] Guid issueId,
        [FromServices] TakeOnWorkHandler handler,
        CancellationToken cancellationToken = default)
    {
        var userId = HttpContext.User.Claims.First(c => c.Type == CustomClaims.Id).Value;

        var command = new TakeOnWorkCommand(Guid.Parse(userId), issueId);

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
}