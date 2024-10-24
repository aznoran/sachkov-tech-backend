using NotificationService.Api.Dto;

namespace NotificationService.Api.Contracts
{
    public record PushNotificationRequest(
        MessageDto msg,
        Guid[] UserIds,
        Guid[] RoleIds);
}
