namespace EmailNotificationService.API.Common;

public record Response
{
    public bool Success { get; init; }
    
    public string Message { get; init; } = string.Empty;
    
    public int StatusCode { get; init; }
}