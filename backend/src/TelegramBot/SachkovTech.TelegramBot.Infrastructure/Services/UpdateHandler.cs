using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace SachkovTech.TelegramBot.Infrastructure.Services;

public class UpdateHandler : IUpdateHandler
{
    private readonly ILogger<UpdateHandler> _logger;
    private readonly ITelegramBotClient _botClient;

    public UpdateHandler(
        ITelegramBotClient botClient,
        ILogger<UpdateHandler> logger)
    {
        _botClient = botClient;
        _logger = logger;
    }
    
    public async Task HandleUpdateAsync(
        ITelegramBotClient botClient, 
        Update update, 
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        await (update switch
        {
            { Message: { } message } => OnMessage(message),
            _ => UnknownUpdateHandler(update)
        });
    }

    private async Task OnMessage(Message message)
    {
        _logger.LogInformation("Receive message type: {MessageType}", message.Type);

        if (message.Text.IsNullOrEmpty())
            return;

        await _botClient.SendTextMessageAsync(message.Chat, $"Сообщение было получено: {message.Text}");
    }

    private Task UnknownUpdateHandler(Update update)
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
}