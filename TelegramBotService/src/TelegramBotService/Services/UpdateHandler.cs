using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using TelegramBotService.Core.Models;
using TelegramBotService.Handlers.Commands;
using TelegramBotService.Handlers.Messages;
using TelegramBotService.MongoDataAccess;
using TelegramBotService.StateMachine;

namespace TelegramBotService.Services;

public class UpdateHandler : IUpdateHandler
{
    private readonly ILogger<UpdateHandler> _logger;
    private readonly ITelegramBotClient _botClient;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IUserStateRepository _userStateRepository;

    public UpdateHandler(
        ITelegramBotClient botClient,
        ILogger<UpdateHandler> logger,
        IServiceScopeFactory serviceScopeFactory,
        IUserStateRepository userStateRepository)
    {
        _botClient = botClient;
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _userStateRepository = userStateRepository;
    }
    
    public async Task HandleUpdateAsync(
        ITelegramBotClient botClient, 
        Update update, 
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        await (update switch
        {
            { Message: { } message } => ProcessMessage(message, cancellationToken),
            _ => UnknownUpdateHandler(update, cancellationToken)
        });
    }

    private async Task ProcessMessage(Message message, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Receive message type: {MessageType}", message.Type);
        
        if (string.IsNullOrEmpty(message.Text))
            return;

        await CheckForUserState(message, cancellationToken);
        
        var scope = _serviceScopeFactory.CreateScope();
        
        if (message.Text.StartsWith('/'))
        {
            var handler = scope.ServiceProvider.GetService<OnCommandHandler>();
            
            await handler!.HandleAsync(message);
        }
        else
        {
            var handler = scope.ServiceProvider.GetService<OnMessageHandler>();
            
            await handler!.HandleAsync(message);
        }
    }

    private Task UnknownUpdateHandler(Update update, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Unknown update type {UpdateType}:", update.Type);

        return Task.CompletedTask;
    }

    public async Task HandlePollingErrorAsync(
        ITelegramBotClient botClient, 
        Exception exception, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("HandleError: {exception}", exception);

        if (exception is RequestException)
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
    }

    private async Task CheckForUserState(Message message, CancellationToken cancellationToken)
    {
        var userState = await _userStateRepository.Get(message.Chat.Id, cancellationToken);

        if (userState is null)
        {
            UserState newState = new UserState()
            {
                ChatId = message.Chat.Id,
                State = 0
            };
            
            await _userStateRepository.Add(newState, cancellationToken);
        }
    }
}