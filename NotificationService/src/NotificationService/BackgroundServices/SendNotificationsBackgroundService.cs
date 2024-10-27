using NotificationService.BackgroundServices.Services;

namespace NotificationService.BackgroundServices;

internal class SendNotificationsBackgroundService : BackgroundService
{
    private const int SEND_NOTIFICATIONS_SERVICE_REDUCTION_SECONDS = 1;
    private readonly ILogger<SendNotificationsBackgroundService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public SendNotificationsBackgroundService(
        ILogger<SendNotificationsBackgroundService> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("SendNotificationsBackgroundService is started");

        while (!cancellationToken.IsCancellationRequested)
        {
            await using var scope = _scopeFactory.CreateAsyncScope();

            var sendNotificationsService = scope.ServiceProvider
                .GetRequiredService<SendNotificationsService>();

            _logger.LogInformation("SendNotificationsService is working");

            try
            {
                await sendNotificationsService.Process(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError("SendNotificationsService failed with error: {exception}", ex.Message);
            }

            await Task.Delay(
                TimeSpan.FromSeconds(SEND_NOTIFICATIONS_SERVICE_REDUCTION_SECONDS),
                cancellationToken);
        }
    }
}