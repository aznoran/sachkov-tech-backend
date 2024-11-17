using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotService.Abstractions;

namespace TelegramBotService.StateMachine.States;

public class LoginPasswordState : IState
{
    private readonly IUserStateMachineFactory _stateMachine;
    private readonly long _chatId;
    private readonly ITelegramBotClient _botClient;

    public LoginPasswordState(IUserStateMachineFactory stateMachine, long chatId, ITelegramBotClient botClient)
    {
        _stateMachine = stateMachine;
        _chatId = chatId;
        _botClient = botClient;
    }

    public async Task Enter()
    {
        await _botClient.SendTextMessageAsync(_chatId, 
            "Введи свой пароль.");
    }

    public async Task HandleMessage(Message message)
    {
        if (message.Text!.Length > 100)
        {
            await _botClient.SendTextMessageAsync(_chatId, 
                "Указывай реальные данные.");
            return;
        }
        
        //получаем имя пользователя через монгу, вместе с паролем отсылаем на auth service 

        var newStateMachine = _stateMachine.Create(_chatId);
        await newStateMachine.ChangeState(new MainMenuState(_stateMachine, _chatId, _botClient));
    }

    public async Task Exit()
    {
        await _botClient.SendTextMessageAsync(_chatId, 
            "Ты успешно авторизован. Спасибо.");
    }
}