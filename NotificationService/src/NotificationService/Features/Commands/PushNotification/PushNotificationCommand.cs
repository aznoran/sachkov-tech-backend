using NotificationService.Grpc;

namespace NotificationService.Features.Commands.PushNotification;

public record PushNotificationCommand(
    MessageDto Msg,
    Guid[] UserIds,
    Guid[] Roles);
