namespace NotificationService.Features.Commands.PushNotification;

public record PushNotificationCommand(
    Grpc.MessageDto Msg,
    Guid[] UserIds,
    Guid[] Roles);