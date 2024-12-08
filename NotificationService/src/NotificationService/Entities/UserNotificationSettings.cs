namespace NotificationService.Entities;

public class UserNotificationSettings
{
    public Guid Id { get; private set; }
    public required Guid UserId { get; set; }
    public bool SendEmail { get; set; } = true;
    public bool SendTelegram { get; set; }
    public bool SendWeb { get; set; }
}