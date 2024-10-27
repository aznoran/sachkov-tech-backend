namespace NotificationService.HelperClasses;

public record Error
{
    public const string SEPARATOR = "||";

    public string Code { get; }
    public string Message { get; }
    public ErrorType Type { get; }
    public string? InvalidField { get; }

    private Error(string code, string message, ErrorType type, string? invalidField = null)
    {
        Code = code;
        Message = message;
        Type = type;
        InvalidField = invalidField;
    }

    public static Error Validation(string message, string code = "value.is.invalid", string? invalidField = null) =>
        new(code, message, ErrorType.Validation, invalidField);

    public static Error NotFound(string message, string code = "value.not.found") => new(code, message, ErrorType.NotFound);

    public static Error Failure(string code, string message) => new(code, message, ErrorType.Failure);

    public static Error Conflict(string code, string message) => new(code, message, ErrorType.Conflict);

    public string Serialize()
    {
        return string.Join(SEPARATOR, Code, Message, Type);
    }

    public enum ErrorType
    {
        Validation,
        NotFound,
        Failure,
        Conflict
    }
}