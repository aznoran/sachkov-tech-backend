using Microsoft.Extensions.Logging;
using SachkovTech.TelegramBot.Infrastructure.Abstractions;
using Telegram.Bot;

namespace SachkovTech.TelegramBot.Infrastructure.Services;

public class ReceiverService(
    ITelegramBotClient botClient, 
    UpdateHandler updateHandler, 
    ILogger<ReceiverServiceBase<UpdateHandler>> logger)
    : ReceiverServiceBase<UpdateHandler>(botClient, updateHandler, logger);