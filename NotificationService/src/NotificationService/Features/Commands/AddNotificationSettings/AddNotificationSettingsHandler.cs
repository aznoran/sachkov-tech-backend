using CSharpFunctionalExtensions;
using NotificationService.Entities;
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
            var dbSet = _dbContext.Set<NotificationSettings>();

            var notificationSettings = new NotificationSettings()
            {
                Id = command.Id,
                UserId = command.UserId
            };

            notificationSettings.UpdatePreferences(
                email: command.Email,
                telegram: command.Telegram,
                web: command.Web);

            await dbSet.AddAsync(notificationSettings);
            await _dbContext.SaveChangesAsync();

            return Result.Success<Error>();
        }
    }
}
