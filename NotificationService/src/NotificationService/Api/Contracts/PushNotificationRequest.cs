using NotificationService.Api.Dto;

namespace NotificationService.Api.Contracts;

public record PushNotificationRequest(
    MessageDto Message,
    Guid[] UserIds,
    Guid[] RoleIds);