namespace NotificationService.Api.Contracts;

public record PatchNotificationSettingsRequest(
    string NotificationType,
    bool Value);