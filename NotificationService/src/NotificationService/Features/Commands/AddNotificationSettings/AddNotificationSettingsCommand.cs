namespace NotificationService.Features.Commands
{
    public record AddNotificationSettingsCommand(
        Guid Id,
        Guid UserId,
        string Email,
        string WebEndpoint);
}
