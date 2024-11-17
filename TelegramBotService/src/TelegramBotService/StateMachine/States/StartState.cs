using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotService.Abstractions;

namespace TelegramBotService.StateMachine.States;

public class StartState : IState
{
    private readonly IUserStateMachineFactory _stateMachine;
    private readonly long _chatId;
    private readonly ITelegramBotClient _botClient;
    
    public StartState(IUserStateMachineFactory stateMachine, long chatId, ITelegramBotClient botClient)
    {
        _stateMachine = stateMachine;
        _chatId = chatId;
        _botClient = botClient;
    }

    public async Task Enter()
    {
        await _botClient.SendTextMessageAsync(_chatId, 
            "Привет! Введи /help для просмотра доступных команд.");
    }

    public async Task HandleMessage(Message message)
    {
        await _botClient.SendTextMessageAsync(_chatId, 
            "Привет! Введи /help для просмотра доступных команд.");
    }

    public Task Exit()
    {
        return Task.CompletedTask;
    }
}