namespace TelegramBotService.Options;

public class TelegramBotOptions
{
    public const string TELEGRAM_BOT = "TelegramBot";

    public string BotToken { get; init; } = default!;
}