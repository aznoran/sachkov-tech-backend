using Grpc.Core;
using NotificationService.Extensions;
using NotificationService.Features.Commands.AddNotificationSettings;
using NotificationService.Features.Commands.PushNotification;

namespace NotificationService.Grpc.Services;

public class NotificationServiceGrpcImplementation : NotificationService.NotificationServiceBase
{
    private readonly IServiceProvider _provider;

    public NotificationServiceGrpcImplementation(IServiceProvider provider)
    {
        _provider = provider;
    }

    public async override Task<GuidGrpc> Add(
        AddNotificationSettingsRequest request,
        ServerCallContext context)
    {
        var cancellationToken = context.CancellationToken;

        var scoped = _provider.CreateScope();
        var handler = scoped.ServiceProvider.GetRequiredService<AddNotificationSettingsHandler>();

        if (request.UserId.IsValidGuid(out RpcException exception) == false)
            throw exception;

        var command = new AddNotificationSettingsCommand
            (
                new Guid(request.UserId.Guid),
                request.Email,
                request.HasWebEndpoint ? request.WebEndpoint : null
            );

        var result = await handler.Handle(command, cancellationToken);
        if (result.IsFailure)
            throw result.Error.ToGrpcResponse();

        var response = new GuidGrpc() { Guid = result.Value.ToString() };
        return await Task.FromResult(response);
    }

    public async override Task<GuidGrpc> Push(
        PushNotificationRequest request,
        ServerCallContext context)
    {
        var cancellationToken = context.CancellationToken;

        var scoped = _provider.CreateScope();
        var handler = scoped.ServiceProvider.GetRequiredService<PushNotificationHandler>();

        if (request.UserIds.IsValidGuid(out RpcException exceptionA, nameof(request.UserIds)) == false)
            throw exceptionA;
        if (request.RoleIds.IsValidGuid(out RpcException exceptionB, nameof(request.RoleIds)) == false)
            throw exceptionB;

        var command = new PushNotificationCommand(
            request.Message,
            request.UserIds.Select(x => new Guid(x.Guid)).ToArray(),
            request.RoleIds.Select(x => new Guid(x.Guid)).ToArray());

        var result = await handler.Handle(command, cancellationToken);

        if (result.IsFailure)
            throw result.Error.ToGrpcResponse();

        var response = new GuidGrpc() { Guid = result.Value.ToString() };
        return await Task.FromResult(response);
    }
}
