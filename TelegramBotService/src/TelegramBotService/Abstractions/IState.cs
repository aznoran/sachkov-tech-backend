using Telegram.Bot.Types;

namespace TelegramBotService.Abstractions;

public interface IState
{
    Task Enter();
    Task HandleMessage(Message message);
    Task Exit();
}