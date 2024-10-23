using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using NotificationService.Entities;
using NotificationService.HelperClasses;
using NotificationService.Infrastructure;

namespace NotificationService.Features.Commands
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

            var notification = new Notification()
            {
                Id = Guid.NewGuid(),
                RoleIds = command.Roles.ToList(),
                UserIds = command.UserIds.ToList(),
                Message = messageData,
                CreatedAt = DateTime.UtcNow,
                Status = NotificationStatusEnum.Pending
            };

            await _dbContext.Notifications.AddAsync(notification);
            await _dbContext.SaveChangesAsync();

            return notification.Id;
        }
    }

}
