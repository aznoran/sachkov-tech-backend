using Microsoft.EntityFrameworkCore;

using NotificationService.BackgroundServices;
using NotificationService.BackgroundServices.Channels;
using NotificationService.BackgroundServices.Factories;
using NotificationService.BackgroundServices.Services;
using NotificationService.Features.Commands;
using NotificationService.Features.Commands.AddNotificationSettings;
using NotificationService.Features.Commands.PatchNotificationSettings;
using NotificationService.Features.Commands.PushNotification;
using NotificationService.Features.Queries;
using NotificationService.Features.Queries.GetNotificationSettings;
using NotificationService.Infrastructure;

namespace NotificationService.Extensions;

public static class AppExtensions
{
    public static async Task AddMigrations(this WebApplication app)
    {
        await using var scoped = app.Services.CreateAsyncScope();
        var dbContext = scoped.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        //await dbContext.Database.MigrateAsync();
    }

    public static void AddHandlers(this IServiceCollection services)
    {
        services.AddScoped<AddNotificationSettingsHandler>();
        services.AddScoped<PatchNotificationSettingsHandler>();
        services.AddScoped<GetNotificationSettingsHandler>();
        services.AddScoped<PushNotificationHandler>();
    }
    
    public static void AddBackgroundService(this IServiceCollection services)
    {
        services.AddHostedService<SendNotificationsBackgroundService>();
        services.AddScoped<SendNotificationsService>();

        services.AddScoped<INotificationSender, TelegramNotificationChannel>();
        services.AddScoped<INotificationSender, WebNotificationChannel>();
        services.AddScoped<INotificationSender, EmailNotificationChannel>();
        
        services.AddScoped<NotificationSettingsFactory>(provider =>
        {
            var senders = provider.GetService<IEnumerable<INotificationSender>>();
            return new NotificationSettingsFactory(senders!);
        });
    }
}