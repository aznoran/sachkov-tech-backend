namespace NotificationService.Features.Commands
{
    public record PatchNotificationSettingsCommand(
        Guid Id,
        string NotificationType,
        bool Value);
}
