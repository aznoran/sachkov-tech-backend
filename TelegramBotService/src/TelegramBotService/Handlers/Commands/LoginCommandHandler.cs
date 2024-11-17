using Telegram.Bot;
using TelegramBotService.Abstractions;
using TelegramBotService.Factory;
using TelegramBotService.MongoDataAccess;
using TelegramBotService.StateMachine;
using TelegramBotService.StateMachine.States;

namespace TelegramBotService.Handlers.Commands;

public class LoginCommandHandler : ICommand
{
    private readonly ITelegramBotClient _botClient;
    private readonly IUserStateMachineFactory _userStateMachineFactory;
    private readonly UserStateFactory _factory;
    private readonly IUserStateRepository _userStateRepository;

    public LoginCommandHandler(ITelegramBotClient botClient,
        IUserStateMachineFactory userStateMachineFactory,
        UserStateFactory factory,
        IUserStateRepository userStateRepository)
    {
        _botClient = botClient;
        _userStateMachineFactory = userStateMachineFactory;
        _factory = factory;
        _userStateRepository = userStateRepository;
    }

    public async Task ExecuteAsync(Telegram.Bot.Types.Message message)
    {
        var userState = await _userStateRepository.Get(message.Chat.Id);
        var state = _factory.GetState(message.Chat.Id, userState.State);

        if (state is not StartState)
        {
            await _botClient.SendTextMessageAsync(message.Chat, "Вы уже вошли в систему");
            return;
        }

        var userStateMachine = _userStateMachineFactory.Create(message.Chat.Id);
        
        await userStateMachine.ChangeState(_factory.GetState(message.Chat.Id, 1));
    }
}