using System.Text;
using Telegram.Bot;
using TelegramBotService.Abstractions;

namespace TelegramBotService.Handlers.Commands;

public class HelpCommandHandler : ICommand
{
    private readonly ITelegramBotClient _botClient;

    public HelpCommandHandler(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task ExecuteAsync(Telegram.Bot.Types.Message message)
    {
        var stringBuild = new StringBuilder();
        stringBuild.Append("Доступные команды:\n");
        
        var type = typeof(ICommand);
        
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => type.IsAssignableFrom(p) && p != type);

        foreach (var inputType in types)
        {
            stringBuild.Append('/' + inputType.Name.Replace("CommandHandler", "").ToLower() + '\n');
        }
        
        await _botClient.SendTextMessageAsync(message.Chat, stringBuild.ToString());
    }
}