using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using NotificationService.Entities;
using NotificationService.HelperClasses;
using NotificationService.Infrastructure;

namespace NotificationService.Features.Queries.GetNotificationSettings;

public class GetNotificationSettingsHandler
{
    private readonly ApplicationDbContext _dbContext;

    public GetNotificationSettingsHandler(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<NotificationSettings, Error>> Handle(
        GetNotificationSettingsQuery query,
        CancellationToken cancellationToken = default)
    {
        var notificationSettings = await _dbContext.NotificationSettings
            .FirstOrDefaultAsync(x => x.Id == query.Id, cancellationToken);

        if (notificationSettings == null)
            return Error.NotFound($"No settings were found with id: {query.Id}",
                "notification.settings.not.found");

        return notificationSettings;
    }
}