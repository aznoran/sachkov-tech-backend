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
        public async Task<Result<NotificationSettings, Error>> Handle(
            PatchNotificationSettingsCommand command,
            CancellationToken cancellationToken = default)
        {
            var dbSet = _dbContext.Set<NotificationSettings>();

            var userSettings = await dbSet.FirstOrDefaultAsync(
                            x => x.Id == command.Id,
                            cancellationToken);

            if (userSettings == null)
                return Error.NotFound("notification.settings.not.found", $"No settings were found with id: {command.Id}");

            var updateRes = UpdateSettings(command.NotificationType, command.Value, userSettings);
            if (updateRes.IsFailure)
                return updateRes.Error;

            await _dbContext.SaveChangesAsync();

            return userSettings;
        }

        private UnitResult<Error> UpdateSettings(string propertyName, bool value, NotificationSettings userSettings)
        {
            switch (propertyName.Trim().ToLower())
            {
                case "email":
                    {
                        userSettings.UpdatePreferences(email: value);
                        break;
                    }
                case "telegram":
                    {
                        userSettings.UpdatePreferences(telegram: value);
                        break;
                    }
                case "web":
                    {
                        userSettings.UpdatePreferences(web: value);
                        break;
                    }
                default:
                    {
                        return Error.Validation("notification.type.not.found",
                            $"No such notification method exists: {propertyName}");
                    }
            }

            return Result.Success<Error>();
        }
    }
}
