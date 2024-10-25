using Grpc.Core;
using NotificationService.Features.Commands.AddNotificationSettings;
using NotificationService.Features.Commands.PushNotification;
using MessageDtoClass = NotificationService.Api.Dto.MessageDto;

namespace NotificationService.Grpc.Services;

public class NotificationServiceGrpcImplementation : NotificationService.NotificationServiceBase
{
    private readonly IServiceProvider _provider;

    public NotificationServiceGrpcImplementation(IServiceProvider provider)
    {
        _provider = provider;
    }

    public async override Task<Guid> Add(
        AddNotificationSettingsRequest request,
        ServerCallContext context)
    {
        var cancellationToken = context.CancellationToken;

        var scoped = _provider.CreateScope();
        var handler = scoped.ServiceProvider.GetRequiredService<AddNotificationSettingsHandler>();

        var command = new AddNotificationSettingsCommand
            (
                new System.Guid(request.UserId.Guid_),
                request.Email,
                request.HasWebEndpoint ? request.WebEndpoint : null
            );

        var result = await handler.Handle(command, cancellationToken); // todo error handling
        //if (result.IsFailure)
        //    return result.Error.ToResponse(); // todo
        /*
            response = new AddResponse()
            {
                result = null;
            }
        */

        /*
        response = new AddResponse()
            {
                result = handle.Result;
            }
        */

        var response = new Guid() { Guid_ = result.Value.ToString() };
        return await Task.FromResult(response);
    }

    public async override Task<Guid> Push(
        PushNotificationRequest request,
        ServerCallContext context)
    {
        var cancellationToken = context.CancellationToken;

        var scoped = _provider.CreateScope();
        var handler = scoped.ServiceProvider.GetRequiredService<PushNotificationHandler>();

        var command = new PushNotificationCommand(
            new MessageDtoClass(request.Message.Title, request.Message.Message),
            request.UserIds.Select(x => new System.Guid(x.Guid_)).ToArray(),
            request.RoleIds.Select(x => new System.Guid(x.Guid_)).ToArray());

        var result = await handler.Handle(command, cancellationToken);

        //if (result.IsFailure) //todo
        //    return result.Error.ToResponse();

        var response = new Guid() { Guid_ = result.Value.ToString() };
        return await Task.FromResult(response);
    }
}
