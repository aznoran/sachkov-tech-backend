using TelegramBotService.Abstractions;
using TelegramBotService.MongoDataAccess;
using TelegramBotService.StateMachine;

namespace TelegramBotService.Factory;

public class UserStateMachineFactory : IUserStateMachineFactory
{
    private readonly IUserStateProvider _userStateProvider;
    private readonly IUserStateRepository _userStateRepository;

    public UserStateMachineFactory(IUserStateProvider userStateProvider,
        IUserStateRepository userStateRepository)
    {
        _userStateProvider = userStateProvider;
        _userStateRepository = userStateRepository;
    }

    public UserStateMachine Create(long chatId)
    {
        return new UserStateMachine(chatId, _userStateRepository, _userStateProvider);
    }
}