using NotificationService.Grpc;

namespace NotificationService.Api.Contracts;

public record PushNotificationRequest(
    MessageDto Message,
    Guid[] UserIds,
    Guid[] RoleIds);