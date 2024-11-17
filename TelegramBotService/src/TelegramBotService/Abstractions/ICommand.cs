namespace TelegramBotService.Abstractions;

public interface ICommand
{
    Task ExecuteAsync(Telegram.Bot.Types.Message message);
}