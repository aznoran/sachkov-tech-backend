namespace NotificationService.Features.Commands
{
    public record AddNotificationSettingsCommand(
        Guid Id,
        Guid UserId,
        bool? Email = null,
        bool? Telegram = null,
        bool? Web = null);
}
