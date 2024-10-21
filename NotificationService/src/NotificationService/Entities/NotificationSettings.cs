namespace NotificationService.Entities;

public class NotificationSettings
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    public bool Email { get; set; } = true;

    public bool Telegram { get; set; }

    public bool Web { get; set; } = true;

}