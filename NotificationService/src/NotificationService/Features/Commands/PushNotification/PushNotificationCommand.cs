using NotificationService.Api.Dto;

namespace NotificationService.Features.Commands.PushNotification;

public record PushNotificationCommand(
    MessageDto Msg,
    Guid[] UserIds,
    Guid[] Roles);