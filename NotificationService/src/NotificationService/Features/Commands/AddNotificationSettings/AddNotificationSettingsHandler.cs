using CSharpFunctionalExtensions;
using NotificationService.Entities;
using NotificationService.Extensions;
using NotificationService.HelperClasses;
using NotificationService.Infrastructure;

namespace NotificationService.Features.Commands
{
    public class AddNotificationSettingsHandler
    {
        private readonly ApplicationDbContext _dbContext;
        public AddNotificationSettingsHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<UnitResult<Error>> Handle(
            AddNotificationSettingsCommand command,
            CancellationToken cancellationToken = default)
        {
            var notificationSettings = new NotificationSettings()
            {
                Id = command.Id,
                UserId = command.UserId
            };

            notificationSettings.Email = command.Email ?? notificationSettings.Email;
            notificationSettings.Telegram = command.Telegram ?? notificationSettings.Telegram;
            notificationSettings.Web = command.Web ?? notificationSettings.Web;

            await _dbContext.NotificationSettings.AddAsync(notificationSettings);
            await _dbContext.SaveChangesAsync();

            return Result.Success<Error>();
        }
    }
}
