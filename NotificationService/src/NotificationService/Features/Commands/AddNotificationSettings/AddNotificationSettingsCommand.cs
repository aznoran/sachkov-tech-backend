namespace NotificationService.Features.Commands
{
    public record AddNotificationSettingsCommand(
        Guid UserId,
        string Email,
        string? WebEndpoint);
}
