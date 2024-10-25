using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using NotificationService.Entities;
using NotificationService.HelperClasses;
using NotificationService.Infrastructure;

namespace NotificationService.Features.Commands.PatchNotificationSettings;

public class PatchNotificationSettingsHandler
{
    private readonly ApplicationDbContext _dbContext;
    public PatchNotificationSettingsHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<UnitResult<Error>> Handle(
        PatchNotificationSettingsCommand command,
        CancellationToken cancellationToken = default)
    {
        var notificationSettings = await _dbContext.NotificationSettings
            .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

        if (notificationSettings == null)
            return Error.NotFound($"No settings were found with id: {command.Id}",
                "notification.settings.not.found");

        var updateRes = UpdateSettings(
            notificationSettings,
            command.NotificationType,
            command.Value);

        if (updateRes.IsFailure)
            return updateRes.Error;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success<Error>();
    }

    private UnitResult<Error> UpdateSettings(NotificationSettings userSettings, string propertyName, bool value)
    {
        switch (propertyName.Trim().ToLower())
        {
            case "email":
                {
                    return userSettings.UseEmailNotifications(value);
                }
            case "telegram":
                {
                    return userSettings.UseTelegramNotifications(value);
                }
            case "web":
                {
                    return userSettings.UseWebNotifications(value);
                }
            default:
                {
                    return Error.Validation($"No such notification method exists: {propertyName}",
                        "invalid.value.notification.type");
                }
        }
    }
}