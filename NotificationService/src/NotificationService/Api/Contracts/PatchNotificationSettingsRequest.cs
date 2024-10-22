namespace NotificationService.Api
{
    public record PatchNotificationSettingsRequest(
        string NotificationType,
        bool Value,
        string? ConnectionPath = null);
}
