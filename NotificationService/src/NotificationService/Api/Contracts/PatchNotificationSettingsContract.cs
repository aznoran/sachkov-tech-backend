namespace NotificationService.Api
{
    public record PatchNotificationSettingsContract(
        string NotificationType,
        bool Value);
}
