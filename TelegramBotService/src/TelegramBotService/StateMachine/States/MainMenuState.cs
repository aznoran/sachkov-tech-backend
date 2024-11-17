using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotService.Abstractions;

namespace TelegramBotService.StateMachine.States;

public class MainMenuState : IState
{
    private readonly IUserStateMachineFactory _stateMachine;
    private readonly long _chatId;
    private readonly ITelegramBotClient _botClient;

    public MainMenuState(IUserStateMachineFactory stateMachine, long chatId, ITelegramBotClient botClient)
    {
        _stateMachine = stateMachine;
        _chatId = chatId;
        _botClient = botClient;
    }

    public async Task Enter()
    {
        await _botClient.SendTextMessageAsync(_chatId, 
            "Главное меню.");
    }

    public Task HandleMessage(Message message)
    {
        return Task.CompletedTask;
    }

    public Task Exit()
    {
        return Task.CompletedTask;
    }
}