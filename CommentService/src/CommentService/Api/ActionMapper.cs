using CommentService.Api.Contracts.Requests;
using CommentService.Features.Commands.AddComment;
using CommentService.Features.Commands.DeleteComment;
using CommentService.Features.Commands.UpdateComment.MainInfo;
using CommentService.Features.Commands.UpdateComment.RatingDecrease;
using CommentService.Features.Commands.UpdateComment.RatingIncrease;
using CommentService.Features.Queries.GetCommentByRelationId;
using CommentService.HelperClasses;
using Microsoft.AspNetCore.Mvc;

namespace CommentService.Api;

public static class ActionMapper
{
    public static void RegisterCommentActions(this WebApplication app)
    {
        app.MapPost("/comment", async (
            [FromBody] AddCommentRequest request,
            [FromServices] AddCommentHandler handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.Handle(request.ToCommand(), cancellationToken);

            return result.IsFailure
                ? Results.BadRequest(Envelope.Error([result.Error]))
                : Results.Ok(Envelope.Ok(result.Value));
        });

        app.MapDelete("/comment/{id:guid}", async (
            [FromRoute] Guid id,
            [FromServices] DeleteCommentHandler handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.Handle(new DeleteCommentCommand(id), cancellationToken);

            return result.IsFailure
                ? Results.BadRequest(Envelope.Error([result.Error]))
                : Results.Ok();
        });

        app.MapPatch("/comment/{id:guid}/update-main-info/", async (
            [FromRoute] Guid id,
            [FromBody] UpdateMainInfoCommentRequest request,
            [FromServices] UpdateMainInfoCommentHandler handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.Handle(request.ToCommand(id), cancellationToken);

            return result.IsFailure
                ? Results.BadRequest(Envelope.Error([result.Error]))
                : Results.Ok(Envelope.Ok(result.Value));
        });

        app.MapPatch("/comment/{id:guid}/update-rating-increase/", async (
            [FromRoute] Guid id,
            [FromServices] UpdateRatingIncreaseCommentHandler handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.Handle(new UpdateRatingIncreaseCommentCommand(id), cancellationToken);

            return result.IsFailure
                ? Results.BadRequest(Envelope.Error([result.Error]))
                : Results.Ok(Envelope.Ok(result.Value));
        });

        app.MapPatch("/comment/{id:guid}/update-rating-decrease/", async (
            [FromRoute] Guid id,
            [FromServices] UpdateRatingDecreaseCommentHandler handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.Handle(new UpdateRatingDecreaseCommentCommand(id), cancellationToken);

            return result.IsFailure
                ? Results.BadRequest(Envelope.Error([result.Error]))
                : Results.Ok(Envelope.Ok(result.Value));
        });

        app.MapGet("/comments/{relationId}", async (
            [FromRoute] Guid relationId,
            [FromQuery] Guid? cursor,
            [FromQuery] string? sortDirection,
            [FromQuery] int limit,
            [FromServices] GetCommentByRelationIdWithPaginationHandler handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetCommentByRelationIdWithPaginationQuery(relationId, cursor, sortDirection, limit);
            var result = await handler.Handle(query, cancellationToken);

            return Results.Ok(Envelope.Ok(result));
        });
    }
}