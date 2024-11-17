using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotService.Abstractions;

namespace TelegramBotService.StateMachine.States;

public class LoginUserNameState : IState
{
    private readonly IUserStateMachineFactory _stateMachine;
    private readonly long _chatId;
    private readonly ITelegramBotClient _botClient;

    public LoginUserNameState(IUserStateMachineFactory stateMachine, long chatId, ITelegramBotClient botClient)
    {
        _stateMachine = stateMachine;
        _chatId = chatId;
        _botClient = botClient;
    }

    public async Task Enter()
    {
        await _botClient.SendTextMessageAsync(_chatId, 
            "Введите своё имя пользователя.");
    }

    public async Task HandleMessage(Message message)
    {
        if (message.Text!.Length > 100)
        {
            await _botClient.SendTextMessageAsync(_chatId, 
                "Слишком большой логин, указывай реальные данные.");
            return;
        }
        
        //добавление строки в монго для хранения юзера , а именно его имени по чатайди (удалять через 1 час данные)
        
        var newStateMachine = _stateMachine.Create(_chatId);
        await newStateMachine.ChangeState(new LoginPasswordState(_stateMachine, _chatId, _botClient));
    }

    public Task Exit()
    {
        return Task.CompletedTask;
    }
}