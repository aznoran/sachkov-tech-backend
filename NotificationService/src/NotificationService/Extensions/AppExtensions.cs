using Microsoft.EntityFrameworkCore;
using NotificationService.Features.Commands;
using NotificationService.Features.Queries;
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
    }
}