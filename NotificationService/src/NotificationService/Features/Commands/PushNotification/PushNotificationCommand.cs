using NotificationService.Entities;

namespace NotificationService.Features.Commands
{
    public record PushNotificationCommand(
        MessageData Msg,
        Guid[] UserIds,
        Guid[] Roles);
}
