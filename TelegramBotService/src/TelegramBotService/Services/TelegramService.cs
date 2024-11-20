using Telegram.Bot;
using Telegram.Bot.Types;

namespace TelegramBotService.Services;

public class TelegramService : ITelegramService
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<TelegramService> _logger;

    public TelegramService(ITelegramBotClient botClient, ILogger<TelegramService> logger)
    {
        _botClient = botClient;
        _logger = logger;
    }

    public async Task SendTextMessageAsync(ChatId chatId, string text, CancellationToken cancellationToken = default)
    {
        try
        {
            await _botClient.SendTextMessageAsync(chatId, text, cancellationToken: cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while sending text message to chat {ChatId}", chatId);
            throw;
        }
    }
}