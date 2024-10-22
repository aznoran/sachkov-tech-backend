using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using NotificationService.Entities;
using NotificationService.Extensions;
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
            var notificationSettings = await _dbContext.NotificationSettings
                .FirstOrDefaultAsync(x => x.Id == command.Id, cancellationToken);

            if (notificationSettings == null)
                return Error.NotFound("notification.settings.not.found",
                    $"No settings were found with id: {command.Id}");

            var updateRes = UpdateSettings(
                command.NotificationType,
                command.Value,
                notificationSettings);

            if (updateRes.IsFailure)
                return updateRes.Error;

            await _dbContext.SaveChangesAsync();

            return Result.Success<Error>();
        }

        private UnitResult<Error> UpdateSettings(string propertyName, bool value, NotificationSettings userSettings)
        {
            switch (propertyName.Trim().ToLower())
            {
                case "email":
                    {
                        userSettings.Email = value;
                        break;
                    }
                case "telegram":
                    {
                        userSettings.Telegram = value;
                        break;
                    }
                case "web":
                    {
                        userSettings.Web = value;
                        break;
                    }
                default:
                    {
                        return Error.Validation($"No such notification method exists: {propertyName}",
                            "invalid.value.notification.type");
                    }
            }

            return Result.Success<Error>();
        }
    }
}
