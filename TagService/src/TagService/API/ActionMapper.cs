using Microsoft.AspNetCore.Mvc;
using TagService.API.Contracts.Requests;
using TagService.Features.Commands.AddTag;
using TagService.Features.Commands.DeleteTag;
using TagService.Features.Commands.UpdateTag.MainInfo;
using TagService.Features.Commands.UpdateTag.UsagesDecrease;
using TagService.Features.Commands.UpdateTag.UsagesIncrease;
using TagService.Features.Queries.GetTagById;
using TagService.Features.Queries.GetTagsByIdsList;
using TagService.HelperClasses;

namespace TagService.API;

public static class ActionMapper
{
    public static void RegisterTagActions(this WebApplication app)
    {
        app.MapPost("/tag", async (
            [FromBody] AddTagRequest request,
            [FromServices] AddTagHandler handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.Handle(request.ToCommand(), cancellationToken);

            if (result.IsFailure)
                return Results.BadRequest(Envelope.Error([result.Error]));

            return Results.Ok(Envelope.Ok(result.Value));
        });

        app.MapDelete("/tag/{id:guid}", async (
            [FromRoute] Guid id,
            [FromServices] DeleteTagHandler handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.Handle(new DeleteTagCommand(id), cancellationToken);

            if (result.IsFailure)
                return Results.BadRequest(Envelope.Error([result.Error]));

            return Results.Ok();
        });

        app.MapPatch("/tag/{id:guid}/edit", async (
            [FromRoute] Guid id,
            [FromBody] UpdateMainInfoTagRequest request,
            [FromServices] UpdateMainInfoTagHandler handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.Handle(request.ToCommand(id), cancellationToken);
            
            if(result.IsFailure)
                return Results.BadRequest(Envelope.Error([result.Error]));
            
            return Results.Ok(Envelope.Ok(result.Value));
        });
        
        app.MapPatch("/tag/{id:guid}/rating-increase/", async (
            [FromRoute] Guid id,
            [FromServices] UpdateUsagesIncreaseTagHandler handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.Handle(new UpdateUsagesIncreaseTagCommand(id), cancellationToken);

            if(result.IsFailure)
                return Results.BadRequest(Envelope.Error([result.Error]));
            
            return Results.Ok(Envelope.Ok(result.Value));
        });

        app.MapPatch("/tag/{id:guid}/rating-decrease/", async (
            [FromRoute] Guid id,
            [FromServices] UpdateUsagesDecreaseTagHandler handler,
            CancellationToken cancellationToken) =>
        {
            var result = await handler.Handle(new UpdateUsagesDecreaseTagCommand(id), cancellationToken);

            if(result.IsFailure)
                return Results.BadRequest(Envelope.Error([result.Error]));
            
            return Results.Ok(Envelope.Ok(result.Value));
        });
        
        app.MapGet("/tags/{cursorId}", async (
            [FromQuery] Guid? cursor,
            [FromQuery] int limit,
            [FromServices] GetTagByIdHandler handler,
            CancellationToken cancellationToken) =>
        {
            var query = new GetTagsWithPaginationQuery(cursor, limit);
            
            var result = await handler.Handle(query, cancellationToken);

            return Results.Ok(Envelope.Ok(result));
        });
        
        app.MapGet("/tags", async (
            [FromQuery] Guid[]? ids,
            [FromServices] GetTagsByListIdsHandler handler,
            CancellationToken cancellationToken) =>
        {
            if (ids is null || ids.Length == 0)
            {
                return Results.BadRequest("Please provide at least one id.");
            }

            var query = new GetTagsByIdsListQuery(ids.ToList());
            
            var result = await handler.Handle(query, cancellationToken);

            return Results.Ok(Envelope.Ok(result));
        });

    }
}