using Telegram.Bot.Types;
using TelegramBotService.Abstractions;
using TelegramBotService.Core.Models;
using TelegramBotService.Factory;
using TelegramBotService.MongoDataAccess;

namespace TelegramBotService.StateMachine;

public class UserStateMachine
{
    private IState currentState;
    private readonly IUserStateRepository _userStateRepository;
    private readonly IUserStateProvider _userStateProvider;
    private long _chatId;

    public UserStateMachine(long chatId, IUserStateRepository userStateRepository, IUserStateProvider userStateProvider)
    {
        _userStateRepository = userStateRepository;
        _userStateProvider = userStateProvider;
        _chatId = chatId;
        Init();
    }

    public async Task ChangeState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        
        await _userStateRepository.Update(new UserState()
        {
            ChatId = _chatId,
            State = _userStateProvider.GetStateNumber(newState) 
        });
        
        currentState?.Enter();
    }

    public void HandleMessage(Message message)
    {
        currentState?.HandleMessage(message);
    }

    public async void Init()
    {
        var userState = await _userStateRepository.Get(_chatId);
        currentState = _userStateProvider.GetState(userState.ChatId, userState.State);
    }
}