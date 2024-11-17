using Telegram.Bot;
using TelegramBotService.Abstractions;
using TelegramBotService.StateMachine;
using TelegramBotService.StateMachine.States;

namespace TelegramBotService.Factory;


/// <summary>
/// Всеми силами я пытался избежать switch case , но из-за самой сути того, что нужно хранить состояние в бд
/// нету другого выхода (по крайней мере я не придумал) чтобы можно было получать инстансы по нужному стейту в бд
/// </summary>
public class UserStateFactory
{
    private readonly IUserStateProvider _userStateProvider;

    public UserStateFactory(IUserStateProvider userStateProvider)
    {
        _userStateProvider = userStateProvider;
    }

    public IState GetState(long chatId, int stateId)
    {
        return _userStateProvider.GetState(chatId, stateId);
    }
}