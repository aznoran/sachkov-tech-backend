using Microsoft.Extensions.Logging;
using SachkovTech.TelegramBot.Infrastructure.Abstractions;

namespace SachkovTech.TelegramBot.Infrastructure.Services;

public class PollingService(IServiceProvider serviceProvider, ILogger<PollingService> logger)
    : PollingServiceBase<ReceiverService>(serviceProvider, logger);