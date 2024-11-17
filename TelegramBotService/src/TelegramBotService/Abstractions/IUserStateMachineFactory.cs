using TelegramBotService.StateMachine;

namespace TelegramBotService.Abstractions;

public interface IUserStateMachineFactory
{
    UserStateMachine Create(long chatId);
}