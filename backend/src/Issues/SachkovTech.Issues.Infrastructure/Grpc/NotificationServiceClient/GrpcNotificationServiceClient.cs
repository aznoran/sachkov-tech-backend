using Grpc.Net.Client;
using SachkovTech.Issues.Application.Features.GrpcNotificationServiceTester.Commands.AddNotificationSettings;
using SachkovTech.Issues.Application.Features.GrpcNotificationServiceTester.Commands.PushNotification;
using SachkovTech.Issues.Application.Interfaces;
using Protos = SachkovTech.Issues.Grpc.Protos;

namespace SachkovTech.Issues.Infrastructure.Grpc.NotificationServiceClient;
public class GrpcNotificationServiceClient : IGrpcNotificationServiceClient
{
    private Protos.NotificationService.NotificationServiceClient _grpcClient;
    public GrpcNotificationServiceClient(GrpcChannel channel)
    {
        _grpcClient = new Protos.NotificationService.NotificationServiceClient(channel);
    }

    public async Task<Guid> PushNotificationAsync(
        PushNotificationCommand command,
        CancellationToken cancellationToken = default)
    {
        var messageDto = new Protos.MessageDto()
        {
            Message = command.Message.Message,
            Title = command.Message.Title
        };

        var request = new Protos.PushNotificationRequest()
        {
            Message = messageDto,
            UserIds = { command.UserIds.Select(x => new Protos.GuidGrpc() { Guid = x.ToString() }) },
            RoleIds = { command.RoleIds.Select(x => new Protos.GuidGrpc() { Guid = x.ToString() }) },
            Type = command.Type,
            Data = command.Data,
        };

        var result = await _grpcClient.PushNotificationAsync(request, cancellationToken: cancellationToken);

        return new Guid(result.Guid);
    }

    public async Task<Guid> AddNotificationSettingsAsync(
        AddNotificationSettingsCommand command,
        CancellationToken cancellationToken = default)
    {
        var request = new Protos.AddNotificationSettingsRequest()
        {
            Email = command.Email,
            UserId = new Protos.GuidGrpc() { Guid = command.UserId.ToString() },
            WebEndpoint = command.WebEndpoint
        };
        var result = await _grpcClient.AddNotificationSettingsAsync(request, cancellationToken: cancellationToken);

        return new Guid(result.Guid);
    }
}
