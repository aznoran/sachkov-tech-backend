using CSharpFunctionalExtensions;
using NotificationService.Entities;
using NotificationService.Entities.ValueObjects;
using NotificationService.HelperClasses;
using NotificationService.Infrastructure;

namespace NotificationService.Features.Commands.PushNotification
{
    public class PushNotificationHandler
    {
        private readonly ApplicationDbContext _dbContext;
        public PushNotificationHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Result<Guid,Error>> Handle(
            PushNotificationCommand command,
            CancellationToken cancellationToken = default)
        {
            var messageData = new MessageData(
                command.Msg.Title,
                command.Msg.Message);

            var notification = Notification.Create(
                command.Roles.ToList(),
                command.UserIds.ToList(),
                messageData);

            await _dbContext.Notifications.AddAsync(notification, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return notification.Id;
        }
    }

}
