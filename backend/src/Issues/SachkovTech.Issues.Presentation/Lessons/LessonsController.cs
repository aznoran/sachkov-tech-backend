using Microsoft.AspNetCore.Mvc;
using SachkovTech.Framework;
using SachkovTech.Framework.Authorization;
using SachkovTech.Issues.Application.Features.Lessons.Command.AddIssueToLesson;
using SachkovTech.Issues.Application.Features.Lessons.Command.AddLesson;
using SachkovTech.Issues.Application.Features.Lessons.Command.AddTagToLesson;
using SachkovTech.Issues.Application.Features.Lessons.Command.RemoveIssueFromLesson;
using SachkovTech.Issues.Application.Features.Lessons.Command.RestoreLesson;
using SachkovTech.Issues.Application.Features.Lessons.Command.SoftDeleteLesson;
using SachkovTech.Issues.Application.Features.Lessons.Command.UpdateLesson;
using SachkovTech.Issues.Application.Features.Lessons.Queries.GetLessonById;
using SachkovTech.Issues.Application.Features.Lessons.Queries.GetLessonWithPagination;
using SachkovTech.Issues.Presentation.Lessons.Requests;

namespace SachkovTech.Issues.Presentation.Lessons;

public class LessonsController : ApplicationController
{
    [HttpPost]
    [Permission(Permissions.Lessons.CreateLesson)]
    public async Task<IActionResult> CreateLesson(
        [FromBody] AddLessonRequest request,
        [FromServices] AddLessonHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpPut]
    [Permission(Permissions.Lessons.UpdateLesson)]
    public async Task<IActionResult> UpdateLesson(
        [FromBody] UpdateLessonRequest request,
        [FromServices] UpdateLessonHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(request.ToCommand(), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [HttpPatch("{lessonId}/tag")]
    [Permission(Permissions.Lessons.UpdateLesson)]
    public async Task<IActionResult> AddTagToLesson(
        [FromRoute] Guid lessonId,
        [FromBody] Guid tagId,
        [FromServices] AddTagToLessonHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new AddTagToLessonCommand(lessonId, tagId), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [HttpPatch("{lessonId}/issue")]
    [Permission(Permissions.Lessons.UpdateLesson)]
    public async Task<IActionResult> AddIssueToLesson(
        [FromRoute] Guid lessonId,
        [FromBody] Guid issueId,
        [FromServices] AddIssueToLessonHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new AddIssueToLessonCommand(lessonId, issueId), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [HttpDelete("{lessonId}/issue")]
    [Permission(Permissions.Lessons.UpdateLesson)]
    public async Task<IActionResult> RemoveIssueFromLesson(
        [FromRoute] Guid lessonId,
        [FromBody] Guid issueId,
        [FromServices] RemoveIssueFromLessonHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new RemoveIssueFromLessonCommand(lessonId, issueId), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }
    
    [HttpDelete("{lessonId}/tag")]
    [Permission(Permissions.Lessons.UpdateLesson)]
    public async Task<IActionResult> RemoveTagFromLesson(
        [FromRoute] Guid lessonId,
        [FromBody] Guid tagId,
        [FromServices] RemoveIssueFromLessonHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new RemoveIssueFromLessonCommand(lessonId, tagId), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }
    
    [HttpPatch("{lessonId}/restore")]
    [Permission(Permissions.Lessons.UpdateLesson)]
    public async Task<IActionResult> RestoreLesson(
        [FromRoute] Guid lessonId,
        [FromServices] RestoreLessonHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new RestoreLessonCommand(lessonId), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }
    
    [HttpDelete("{lessonId:guid}")]
    [Permission(Permissions.Lessons.DeleteLesson)]
    public async Task<IActionResult> SoftDeleteLesson(
        [FromRoute] Guid lessonId,
        [FromServices] SoftDeleteLessonHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new SoftDeleteLessonCommand(lessonId), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [HttpGet]
    [Permission(Permissions.Lessons.ReadLesson)]
    public async Task<IActionResult> GetLessonsWithPagination(
        [FromQuery] int page,
        [FromQuery] int pageSize,
        [FromServices] GetLessonsWithPaginationHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new GetLessonsWithPaginationValidatorQuery(page, pageSize),
            cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }

    [HttpGet("{lessonId:guid}")]
    [Permission(Permissions.Lessons.ReadLesson)]
    public async Task<IActionResult> GetLessonById(
        [FromRoute] Guid lessonId,
        [FromServices] GetLessonByIdHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.Handle(new GetLessonByIdQuery(lessonId), cancellationToken);

        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok(result.Value);
    }
}
