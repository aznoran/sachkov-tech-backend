using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using NotificationService.Entities;
using NotificationService.HelperClasses;
using NotificationService.Infrastructure;

namespace NotificationService.Features.Commands
{
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
            // todo test depending on whatever method we enable, make sure we have the related communication method route set
            var notificationSettings = await _dbContext.NotificationSettings
                .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

            if (notificationSettings == null)
                return Error.NotFound("notification.settings.not.found",
                    $"No settings were found with id: {command.Id}");

            var updateRes = UpdateSettings(
                notificationSettings,
                command.NotificationType,
                command.Value,
                command.ConnectionPath!);

            if (updateRes.IsFailure)
                return updateRes.Error;

            await _dbContext.SaveChangesAsync();

            return Result.Success<Error>();
        }

        private UnitResult<Error> UpdateSettings(NotificationSettings userSettings, string propertyName, bool value, string? connectionPath = null)
        {
            switch (propertyName.Trim().ToLower())
            {
                case "email":
                    {
                        Email emailValue = null!;

                        if (connectionPath != null)
                        {
                            var emailRes = Email.Create(connectionPath); 
                            if (emailRes.IsFailure)
                                return emailRes.Error;
                            emailValue = emailRes.Value;
                        }

                        return userSettings.SetEmailNotifications(value, emailValue);
                    }
                case "telegram":
                    {
                        return userSettings.SetTelegramNotifications(value, connectionPath);
                    }
                case "web":
                    {
                        return userSettings.SetWebNotifications(value, connectionPath);
                    }
                default:
                    {
                        return Error.Validation($"No such notification method exists: {propertyName}",
                            "invalid.value.notification.type");
                    }
            }
        }
    }
}
