namespace NotificationService.Api.Contracts;

public record PushNotificationRequest(
    Grpc.MessageDto Message,
    Guid[] UserIds,
    Guid[] RoleIds);