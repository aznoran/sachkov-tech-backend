using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SachkovTech.Core.Models;
using SachkovTech.Framework;
using SachkovTech.IssuesReviews.Application.Commands.AddComment;
using SachkovTech.IssuesReviews.Application.Commands.Approve;
using SachkovTech.IssuesReviews.Application.Commands.DeleteComment;
using SachkovTech.IssuesReviews.Application.Commands.SendForRevision;
using SachkovTech.IssuesReviews.Application.Commands.StartReview;
using SachkovTech.IssuesReviews.Application.Queries.GetCommentsWithPagination;
using SachkovTech.IssuesReviews.Contracts.Requests;
using SachkovTech.SharedKernel;

namespace SachkovTech.IssuesReviews.Presentation;

public class IssuesReviewsController : ApplicationController
{
    [HttpGet("{issueReviewId:guid}/comments")]
    public async Task<ActionResult> GetByIssueReviewId(
        [FromServices] GetCommentsWithPaginationHandler handler,
        [FromRoute] Guid issueReviewId,
        [FromQuery] GetCommentsWithPaginationQuery query,
        CancellationToken cancellationToken)
    {
        var result = await handler
            .Handle(query.GetQueryWithId(issueReviewId), cancellationToken);

        return Ok(result);
    }

    [HttpPost("{issueReviewId:guid}/comment")]
    public async Task<ActionResult> Comment(
        [FromServices] AddCommentHandler handler,
        [FromRoute] Guid issueReviewId,
        [FromBody] AddCommentRequest request,
        CancellationToken cancellationToken)
    {
        var userId = HttpContext.User.FindFirstValue(CustomClaims.Id);
        
        if (userId == null)
            return Errors.User.InvalidCredentials().ToResponse();

        var result = await handler.Handle(
            new AddCommentCommand(issueReviewId,
                Guid.Parse(userId),
                request.Message), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut("{issueReviewId:guid}/start-review")]
    public async Task<ActionResult> StartReview(
        [FromServices] StartReviewHandler handler,
        [FromRoute] Guid issueReviewId,
        CancellationToken cancellationToken)
    {
        var userId = HttpContext.User.FindFirstValue(CustomClaims.Id);
        
        if (userId == null)
            return Errors.User.InvalidCredentials().ToResponse();

        var result = await handler.Handle(
            new StartReviewCommand(issueReviewId,
                Guid.Parse(userId)), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpPut("{issueReviewId:guid}/revision")]
    public async Task<ActionResult> SendForRevision(
        [FromServices] SendForRevisionHandler handler,
        [FromRoute] Guid issueReviewId,
        CancellationToken cancellationToken)
    {
        var userId = HttpContext.User.FindFirstValue(CustomClaims.Id);
        
        if (userId == null)
            return Errors.User.InvalidCredentials().ToResponse();
        
        var result = await handler.Handle(
            new SendForRevisionCommand(issueReviewId, Guid.Parse(userId)), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpPut("{issueReviewId:guid}/approval")]
    public async Task<ActionResult> Approve(
        [FromServices] ApproveIssueReviewHandler handler,
        [FromRoute] Guid issueReviewId,
        CancellationToken cancellationToken)
    {
        var userId = HttpContext.User.FindFirstValue(CustomClaims.Id);
        
        if (userId == null)
            return Errors.User.InvalidCredentials().ToResponse();
        
        var result = await handler.Handle(
            new ApproveIssueReviewCommand(issueReviewId, Guid.Parse(userId)), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
    
    [HttpDelete("{issueReviewId:guid}/comment/{commentId:guid}")]
    public async Task<ActionResult> DeleteComment(
        [FromServices] DeleteCommentHandler handler,
        [FromRoute] Guid issueReviewId,
        [FromRoute] Guid commentId,
        CancellationToken cancellationToken)
    {
        var userId = HttpContext.User.FindFirstValue(CustomClaims.Id);
        
        if (userId == null)
            return Errors.User.InvalidCredentials().ToResponse();

        var result = await handler.Handle(
            new DeleteCommentCommand(issueReviewId,
                Guid.Parse(userId),
                commentId), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}