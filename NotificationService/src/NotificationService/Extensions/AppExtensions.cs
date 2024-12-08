using Microsoft.EntityFrameworkCore;
using NotificationService.Features.GetNotificationSettings;
using NotificationService.Features.UpdateUserNotificationSettings;
using NotificationService.Infrastructure;
using NotificationService.Services.Factories;
using NotificationService.Services.Senders;

namespace NotificationService.Extensions;

public static class AppExtensions
{
    public static async Task AddMigrations(
        this WebApplication app,
        CancellationToken cancellationToken = default)
    {
        await using var scoped = app.Services.CreateAsyncScope();
        var dbContext = scoped.ServiceProvider.GetRequiredService<NotificationSettingsDbContext>();

        await dbContext.Database.MigrateAsync(cancellationToken);
    }

    public static void AddHandlers(this IServiceCollection services)
    {
        services.AddScoped<UpdateNotificationSettingsHandler>();
        services.AddScoped<GetNotificationSettingsHandler>();
    }

    public static void AddNotificationService(this IServiceCollection services)
    {
        services.AddScoped<INotificationSender, TelegramNotificationSender>();
        services.AddScoped<INotificationSender, WebNotificationSender>();
        services.AddScoped<INotificationSender, EmailNotificationSender>();

        services.AddScoped<NotificationSettingsFactory>(provider =>
        {
            var senders = provider.GetService<IEnumerable<INotificationSender>>();
            return new NotificationSettingsFactory(senders!);
        });
    }
}