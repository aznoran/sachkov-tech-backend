using NotificationService.Api.Dto;

namespace NotificationService.Api
{
    public record PushNotificationRequest(
        MessageDto msg,
        Guid[] UserIds,
        Guid[] RoleIds);
}
