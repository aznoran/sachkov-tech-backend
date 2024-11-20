using Telegram.Bot.Types;

namespace TelegramBotService.Services;

public interface ITelegramService
{
    Task SendTextMessageAsync(ChatId chatId, string text, CancellationToken cancellationToken = default);
}