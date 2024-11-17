using Telegram.Bot;
using TelegramBotService.Abstractions;
using TelegramBotService.Factory;
using TelegramBotService.StateMachine;

namespace TelegramBotService.Handlers.Messages;

public class OnMessageHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly IUserStateMachineFactory _userStateMachineFactory;

    public OnMessageHandler(ITelegramBotClient botClient, IUserStateMachineFactory userStateMachineFactory)
    {
        _botClient = botClient;
        _userStateMachineFactory = userStateMachineFactory;
    }

    public async Task HandleAsync(Telegram.Bot.Types.Message message)
    {
        var userStateMachine = _userStateMachineFactory.Create(message.Chat.Id);
        userStateMachine.HandleMessage(message);
    }
}