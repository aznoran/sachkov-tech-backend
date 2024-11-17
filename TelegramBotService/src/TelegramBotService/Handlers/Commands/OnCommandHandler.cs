using Telegram.Bot;
using TelegramBotService.Abstractions;

namespace TelegramBotService.Handlers.Commands;

public class OnCommandHandler
{
    private readonly ITelegramBotClient _botClient;
    private readonly IServiceProvider _serviceProvider;
    private readonly Dictionary<string, ICommand> _commands = new();

    public OnCommandHandler(ITelegramBotClient botClient, IServiceProvider serviceProvider)
    {
        _botClient = botClient;
        _serviceProvider = serviceProvider;
        InitCommands();
    }

    public async Task HandleAsync(Telegram.Bot.Types.Message message)
    {
        if (_commands.TryGetValue(message.Text, out var command))
        {
            await command.ExecuteAsync(message);
        }
        else
        {
            await _botClient.SendTextMessageAsync(message.Chat,
                "Неизвестная команда. Используйте /help для получения списка команд.");
        }
    }

    private void InitCommands()
    {
        var type = typeof(ICommand);
        
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(p => type.IsAssignableFrom(p) && !p.IsAbstract && p != type);

        foreach (var inputType in types)
        {
            var commandName = '/' + inputType.Name.Replace("CommandHandler", "").ToLower();
            var commandInstance = (ICommand)_serviceProvider.GetService(inputType)!;
            _commands.Add(commandName, commandInstance);
        }
    }
}