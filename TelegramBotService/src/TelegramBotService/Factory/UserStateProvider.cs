using Telegram.Bot;
using TelegramBotService.Abstractions;
using TelegramBotService.StateMachine.States;

namespace TelegramBotService.Factory;

public interface IUserStateProvider
{
    IState GetState(long chatId, int stateId);
    int GetStateNumber(IState state);
}

public class UserStateProvider : IUserStateProvider
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IUserStateMachineFactory _userStateMachineFactory;

    public UserStateProvider(IServiceScopeFactory serviceScopeFactory,
        IUserStateMachineFactory userStateMachineFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _userStateMachineFactory = userStateMachineFactory;
    }

    public IState GetState(long chatId, int stateId)
    {
        var scope = _serviceScopeFactory.CreateScope();
        
        var telegramBotClient = scope.ServiceProvider.GetService<ITelegramBotClient>();
        
        switch (stateId)
        {
            case 0:
                return new StartState(_userStateMachineFactory, chatId, telegramBotClient);
            case 1:
                return new LoginUserNameState(_userStateMachineFactory, chatId, telegramBotClient);
            case 2:
                return new LoginPasswordState(_userStateMachineFactory, chatId, telegramBotClient);
            case 3:
                return new MainMenuState(_userStateMachineFactory, chatId, telegramBotClient);
            default:
                throw new ArgumentException("Invalid state ID");
        }
    }
    
    public int GetStateNumber(IState state)
    {
        switch (state)
        {
            case StartState:
                return 1;
            case LoginUserNameState:
                return 2;
            case LoginPasswordState:
                return 3;
            case MainMenuState:
                return 4;
            default:
                throw new ArgumentException("Invalid state");
        }
    }
}