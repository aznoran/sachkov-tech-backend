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
        var preferences = new NotificationSettings(id, userId, emailAddress);

        UnitResult<Error> setFieldRes = null!;

        setFieldRes = preferences
            .SetEmailNotifications(sendEmail, emailAddress);
        if (setFieldRes.IsFailure)
            return setFieldRes.Error;

        setFieldRes = preferences
            .SetTelegramNotifications(sendTelegram, telegramId);
        if (setFieldRes.IsFailure)
            return setFieldRes.Error;

        setFieldRes = preferences
            .SetWebNotifications(sendWeb, webEndpoint);
        if (setFieldRes.IsFailure)
            return setFieldRes.Error;

        return preferences;
    }

    public UnitResult<Error> SetEmailNotifications(bool value, Email? email = null)
    {
        if (value)
            if (EmailAddress == null && email == null)
                return Error.Validation("Can not use email notification method without email specified!",
                    "invalid.value.notification.setting.email");

        if (email != null)
            EmailAddress = email;

        SendEmail = value;
        return Result.Success<Error>();
    }

    public UnitResult<Error> SetTelegramNotifications(bool value, string? telegramId = null)
    {
        if (value)
            if (String.IsNullOrWhiteSpace(TelegramId) && String.IsNullOrWhiteSpace(telegramId))
                return Error.Validation("Can not use telegram notification method without telegram id specified!",
                    "invalid.value.notification.setting.telegram");

        if (telegramId != null)
            TelegramId = telegramId;

        SendTelegram = value;
        return Result.Success<Error>();
    }

    public UnitResult<Error> SetWebNotifications(bool value, string? webEndpoint = null)
    {
        if (value)
            if (String.IsNullOrWhiteSpace(WebEndpoint) && String.IsNullOrWhiteSpace(webEndpoint))
                return Error.Validation("Can not use web notification method without web endpoint specified!",
                    "invalid.value.notification.setting.web");

        if (webEndpoint != null)
            WebEndpoint = webEndpoint;

        SendWeb = value;
        return Result.Success<Error>();
    }
}