using Microsoft.AspNetCore.Mvc;
using NotificationService.Extensions;
using NotificationService.Features.Commands;
using NotificationService.Features.Queries;
using NotificationService.HelperClasses;

namespace NotificationService.Api
{
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

            return Ok();
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
}
