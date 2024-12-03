namespace NotificationService.Features.UpdateUserNotificationSettings;

public record UpdateNotificationSettingsCommand(
    Guid Id,
    string NotificationType,
    bool Value);