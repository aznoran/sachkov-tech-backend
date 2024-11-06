using Grpc.Net.Client;
using SachkovTech.Issues.Application.Interfaces;
using Protos = SachkovTech.Issues.Grpc.Protos;
using static SachkovTech.Issues.Grpc.Protos.NotificationService;
using SachkovTech.Issues.Application.Features.GrpcNotificationServiceTester.Commands.AddNotificationSettings;
using SachkovTech.Issues.Application.Features.GrpcNotificationServiceTester.Commands.PushNotification;

namespace SachkovTech.Issues.Infrastructure.Grpc.Client;
public class GrpcNotificationServiceClient : IGrpcNotificationServiceClient
{
    private NotificationServiceClient _grpcClient;
    public GrpcNotificationServiceClient(GrpcChannel channel)
    {
        _grpcClient = new NotificationServiceClient(channel);
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
