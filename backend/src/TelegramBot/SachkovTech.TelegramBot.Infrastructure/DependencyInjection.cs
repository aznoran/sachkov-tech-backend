using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SachkovTech.TelegramBot.Infrastructure.Options;
using SachkovTech.TelegramBot.Infrastructure.Services;
using Telegram.Bot;

namespace SachkovTech.TelegramBot.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddTelegramBotInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<TelegramBotOptions>(configuration.GetSection(TelegramBotOptions.TELEGRAM_BOT));

        services.AddHttpClient("telegram_bot_client").RemoveAllLoggers()
            .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
            {
                TelegramBotOptions? options = sp.GetService<IOptions<TelegramBotOptions>>()?.Value;
                ArgumentNullException.ThrowIfNull(options);
                TelegramBotClientOptions botOptions = new(options.BotToken);
                return new TelegramBotClient(botOptions, httpClient);
            });

        services.AddScoped<UpdateHandler>();
        services.AddScoped<ReceiverService>();
        services.AddHostedService<PollingService>();

        return services;
    }
}