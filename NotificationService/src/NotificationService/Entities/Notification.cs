using NotificationService.Entities.ValueObjects;

namespace NotificationService.Entities;

public class Notification
{
    //EF CORE
    private Notification(){}
    private Notification(List<Guid> roleIds,
        List<Guid> userIds,
        MessageData messageData)
    {
        Id = Guid.NewGuid();
        RoleIds = roleIds;
        UserIds = userIds;
        Message = messageData;
        IsSend = false;
        CreatedAt = DateTime.UtcNow;
        Status = NotificationStatusEnum.Pending;
    }
    public Guid Id { get; private set; }

    public List<Guid> RoleIds { get; private set; }

    public List<Guid> UserIds { get; private set; } 

    public MessageData Message { get; private set; } 

    public bool IsSend { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public NotificationStatusEnum Status { get; private set; }

    public static Notification Create(List<Guid> roleIds,
        List<Guid> userIds,
        MessageData messageData)
    {
        return new(roleIds, userIds, messageData);
    }
    public void SetNotificationStatus(NotificationStatusEnum status)
    {
        Status = status;
    }
    
    public void SendingNotificationSuccedeed()
    {
        IsSend = true;
        Status = NotificationStatusEnum.Sent;
    }
    public void SendingNotificationFailed()
    {
        Status = NotificationStatusEnum.Failed;
    }
}

public enum NotificationStatusEnum
{
    Pending,
    Processing,
    Sent,
    Failed
}