using CSharpFunctionalExtensions;
using NotificationService.HelperClasses;

namespace NotificationService.Entities;

public class NotificationSettings
{
    private const string DEFAULT_WEB_ENDPOINT = "localhost:5431";

    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public Email EmailAddress { get; private set; } = null!;

    public bool SendEmail { get; private set; } = true;

    public string TelegramId { get; private set; } = null!;

    public bool SendTelegram { get; private set; }

    public string WebEndpoint { get; private set; } = DEFAULT_WEB_ENDPOINT;

    public bool SendWeb { get; private set; } = true;

    private NotificationSettings(
        Guid id,
        Guid userId,
        Email emailAddress,
        bool sendEmail = true,
        string telegramId = null!,
        bool sendTelegram = false,
        string webEndpoint = DEFAULT_WEB_ENDPOINT,
        bool sendWeb = true)
    {
        Id = id;
        UserId = userId;
        SendEmail = sendEmail;
        EmailAddress = emailAddress;
        SendTelegram = sendTelegram;
        TelegramId = telegramId;
        SendWeb = sendWeb;
        WebEndpoint = webEndpoint;
    }

    public static Result<NotificationSettings, Error> Create(
        Guid id,
        Guid userId,
        Email emailAddress,
        bool sendEmail = true,
        string telegramId = null!,
        bool sendTelegram = false,
        string webEndpoint = DEFAULT_WEB_ENDPOINT,
        bool sendWeb = true)
    {
        var preferences = new NotificationSettings(
            id,
            userId,
            emailAddress: emailAddress,
            telegramId: telegramId,
            webEndpoint: webEndpoint);

        var useNotificationsRes = preferences.UseEmailNotifications(sendEmail);
        if (useNotificationsRes.IsFailure)
            return useNotificationsRes.Error;

        useNotificationsRes = preferences.UseTelegramNotifications(sendTelegram);
        if (useNotificationsRes.IsFailure)
            return useNotificationsRes.Error;

        useNotificationsRes = preferences.UseWebNotifications(sendWeb);
        if (useNotificationsRes.IsFailure)
            return useNotificationsRes.Error;

        return preferences;
    }

    public UnitResult<Error> UseEmailNotifications(bool value)
    {
        if (value && EmailAddress == null)
            return Error.Validation("Can not use email notification method without email specified!",
                "invalid.value.notification.setting.email");

        SendEmail = value;
        return Result.Success<Error>();
    }

    public UnitResult<Error> UseTelegramNotifications(bool value)
    {
        if (value && String.IsNullOrWhiteSpace(TelegramId))
            return Error.Validation("Can not use telegram notification method without telegram id specified!",
                "invalid.value.notification.setting.telegram");

        SendTelegram = value;
        return Result.Success<Error>();
    }

    public UnitResult<Error> UseWebNotifications(bool value)
    {
        if (value && String.IsNullOrWhiteSpace(WebEndpoint))
            return Error.Validation("Can not use web notification method without web endpoint specified!",
                "invalid.value.notification.setting.web");

        SendWeb = value;
        return Result.Success<Error>();
    }
}