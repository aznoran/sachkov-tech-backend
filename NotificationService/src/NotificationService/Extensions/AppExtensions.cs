using Microsoft.EntityFrameworkCore;
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

        await dbContext.Database.MigrateAsync();
    }

    public static void AddHandlers(this IServiceCollection services)
    {
        services.AddScoped<AddNotificationSettingsHandler>();
        services.AddScoped<PatchNotificationSettingsHandler>();
        services.AddScoped<GetNotificationSettingsHandler>();
        services.AddScoped<PushNotificationHandler>();
    }
}