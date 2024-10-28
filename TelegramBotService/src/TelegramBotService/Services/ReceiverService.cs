using Telegram.Bot;
using TelegramBotService.Abstractions;

namespace TelegramBotService.Services;

public class ReceiverService(
    ITelegramBotClient botClient, 
    UpdateHandler updateHandler, 
    ILogger<ReceiverServiceBase<UpdateHandler>> logger)
    : ReceiverServiceBase<UpdateHandler>(botClient, updateHandler, logger);