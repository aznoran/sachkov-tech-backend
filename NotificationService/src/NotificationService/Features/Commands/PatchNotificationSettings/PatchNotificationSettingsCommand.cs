namespace NotificationService.Features.Commands.PatchNotificationSettings;

public record PatchNotificationSettingsCommand(
    Guid Id,
    string NotificationType,
    bool Value);
