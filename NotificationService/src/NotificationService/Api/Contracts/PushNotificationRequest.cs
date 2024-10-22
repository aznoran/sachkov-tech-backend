namespace NotificationService.Api
{
    public record PushNotificationRequest(
        string Title,
        string Message,
        Guid[] UserIds,
        Guid[] Roles);
}
