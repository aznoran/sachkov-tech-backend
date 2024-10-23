using NotificationService.Api.Dto;

namespace NotificationService.Entities;

public class Notification
{
    public Guid Id { get; init; }

    public List<Guid> RoleIds { get; init; } = [];

    public List<Guid> UserIds { get; init; } = [];

    public MessageDto Message { get; init; } = null!;

    public bool IsSend { get; init; }

    public DateTime CreatedAt { get; init; }

    public NotificationStatusEnum Status { get; init; }
}

public enum NotificationStatusEnum
{
    Pending,
    Processing,
    Sent,
    Failed
}