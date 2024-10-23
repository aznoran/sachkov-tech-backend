using NotificationService.Api.Dto;

namespace NotificationService.Features.Commands
{
    public record PushNotificationCommand(
        MessageDto Msg,
        Guid[] UserIds,
        Guid[] Roles);
}
