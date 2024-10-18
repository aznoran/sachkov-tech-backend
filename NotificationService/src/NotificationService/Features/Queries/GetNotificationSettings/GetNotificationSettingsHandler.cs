using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using NotificationService.Entities;
using NotificationService.HelperClasses;
using NotificationService.Infrastructure;

namespace NotificationService.Features.Queries
{
    public class GetNotificationSettingsHandler
    {
        private readonly DbSet<NotificationSettings> _notificationSettings;

        public GetNotificationSettingsHandler(ApplicationDbContext dbContext)
        {
            _notificationSettings = dbContext.Set<NotificationSettings>();
        }

        public async Task<Result<NotificationSettings, Error>> Handle(
            GetNotificationSettingsQuery query,
            CancellationToken cancellationToken = default)
        {
            var notificationSettings = await _notificationSettings.FirstOrDefaultAsync(
                                        x => x.Id == query.Id,
                                        cancellationToken);

            if (notificationSettings == null)
                return Error.NotFound("notification.settings.not.found",
                    $"No settings were found with id: {query.Id}");

            return notificationSettings;
        }
    }
}
