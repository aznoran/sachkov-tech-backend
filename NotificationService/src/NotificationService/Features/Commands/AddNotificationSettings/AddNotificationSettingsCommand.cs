namespace NotificationService.Features.Commands.AddNotificationSettings;

public record AddNotificationSettingsCommand(
    Guid UserId,
    string Email,
    string? WebEndpoint);