namespace EmailNotificationService.API;

public class ExceptionHandler
{
    private readonly RequestDelegate _next;

    public ExceptionHandler(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex) 
        {
            await HandleException(ex, httpContext);
        }
    }
    private async Task HandleException(Exception ex, HttpContext httpContext)
    {

    }
}
