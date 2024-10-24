using Microsoft.AspNetCore.Mvc;
using NotificationService.Api.Contracts;
using NotificationService.Entities;
using NotificationService.Extensions;
using NotificationService.Features.Commands;
using NotificationService.Features.Commands.AddNotificationSettings;
using NotificationService.Features.Commands.PatchNotificationSettings;
using NotificationService.Features.Commands.PushNotification;
using NotificationService.Features.Queries;
using NotificationService.Features.Queries.GetNotificationSettings;
using NotificationService.HelperClasses;

namespace NotificationService.Api;

[Route("[controller]")]
[ApiController]
public class NotificationSettingsController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Add(
        [FromBody] AddNotificationSettingsCommand command,
        [FromServices] AddNotificationSettingsHandler handler,
        CancellationToken cancellationToken = default)
    {
        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        var envelope = Envelope.Ok(result.Value);
        return Ok(envelope);
    }

    [HttpPost("push")]
    public async Task<IActionResult> Push(
        [FromBody] PushNotificationRequest request,
        [FromServices] PushNotificationHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new PushNotificationCommand(
            request.msg,
            request.UserIds,
            request.RoleIds);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        var envelope = Envelope.Ok(result.Value);
        return Ok(envelope);
    }

    [HttpPatch("{id:Guid}")]
    public async Task<IActionResult> Patch(
        [FromRoute] Guid id,
        [FromBody] PatchNotificationSettingsRequest dto,
        [FromServices] PatchNotificationSettingsHandler handler,
        CancellationToken cancellationToken = default)
    {
        var command = new PatchNotificationSettingsCommand(
            id, dto.NotificationType, dto.Value);

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        return Ok();
    }

    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> Get(
        [FromRoute] Guid id,
        [FromServices] GetNotificationSettingsHandler handler,
        CancellationToken cancellationToken = default)
    {
        var query = new GetNotificationSettingsQuery(id);

        var result = await handler.Handle(query,cancellationToken);
        if (result.IsFailure)
            return result.Error.ToResponse();

        var envelope = Envelope.Ok(result.Value);
        return Ok(envelope);
    }
}