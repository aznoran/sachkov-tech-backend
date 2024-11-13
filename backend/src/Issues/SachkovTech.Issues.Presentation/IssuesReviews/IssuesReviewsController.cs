using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SachkovTech.Core.Models;
using SachkovTech.Framework;
using SachkovTech.Framework.Authorization;
using SachkovTech.Framework.Models;
using SachkovTech.Issues.Application.Features.IssuesReviews.Commands.SendForRevision;
using SachkovTech.Issues.Application.Features.IssuesReviews.Commands.AddComment;
using SachkovTech.Issues.Application.Features.IssuesReviews.Commands.Approve;
using SachkovTech.Issues.Application.Features.IssuesReviews.Commands.DeleteComment;
using SachkovTech.Issues.Application.Features.IssuesReviews.Commands.StartReview;
using SachkovTech.Issues.Application.Features.IssuesReviews.Queries.GetCommentsWithPagination;
using SachkovTech.Issues.Contracts.Requests;
using SachkovTech.SharedKernel;

namespace SachkovTech.Issues.Presentation.IssuesReviews;

public class IssuesReviewsController : ApplicationController
{
    [Permission(Permissions.IssuesReview.ReadReviewIssue)]
    [Permission(Permissions.IssuesReview.CommentReviewIssue)]
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

    [Permission(Permissions.IssuesReview.CommentReviewIssue)]
    [HttpPost("{issueReviewId:guid}/comment")]
    public async Task<ActionResult> Comment(
        [FromServices] AddCommentHandler handler,
        [FromRoute] Guid issueReviewId,
        [FromServices] UserScopedData userScopedData,
        [FromBody] AddCommentRequest request,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            new AddCommentCommand(issueReviewId,
                userScopedData.UserId,
                request.Message), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [Permission(Permissions.IssuesReview.CreateReviewIssue)]
    [HttpPut("{issueReviewId:guid}/start-review")]
    public async Task<ActionResult> StartReview(
        [FromServices] StartReviewHandler handler,
        [FromServices] UserScopedData userScopedData,
        [FromRoute] Guid issueReviewId,
        CancellationToken cancellationToken)
    {

        var result = await handler.Handle(
            new StartReviewCommand(issueReviewId, userScopedData.UserId), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [Permission(Permissions.IssuesReview.UpdateReviewIssue)]
    [HttpPut("{issueReviewId:guid}/revision")]
    public async Task<ActionResult> SendForRevision(
        [FromServices] SendForRevisionHandler handler,
        [FromServices] UserScopedData userScopedData,
        [FromRoute] Guid issueReviewId,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            new SendForRevisionCommand(issueReviewId, userScopedData.UserId), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [Permission(Permissions.IssuesReview.UpdateReviewIssue)]
    [HttpPut("{issueReviewId:guid}/approval")]
    public async Task<ActionResult> Approve(
        [FromServices] ApproveIssueReviewHandler handler,
        [FromServices] UserScopedData userScopedData,
        [FromRoute] Guid issueReviewId,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            new ApproveIssueReviewCommand(issueReviewId, userScopedData.UserId), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [Permission(Permissions.IssuesReview.CommentReviewIssue)]
    [HttpDelete("{issueReviewId:guid}/comment/{commentId:guid}")]
    public async Task<ActionResult> DeleteComment(
        [FromServices] DeleteCommentHandler handler,
        [FromServices] UserScopedData userScopedData,
        [FromRoute] Guid issueReviewId,
        [FromRoute] Guid commentId,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(
            new DeleteCommentCommand(issueReviewId,
                userScopedData.UserId,
                commentId), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}