using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace EmailNotificationService.API.Renderer;

public class MailRenderer
{
    private readonly HttpContextAccessor _httpContextAccessor;
    private readonly IRazorViewEngine _viewEngine;
    private readonly ITempDataProvider _tempDataProvider;

    public MailRenderer(
        HttpContextAccessor httpContextAccessor,
        IRazorViewEngine viewEngine,
        ITempDataProvider tempDataProvider)
    {
        _httpContextAccessor = httpContextAccessor;
        _viewEngine = viewEngine;
        _tempDataProvider = tempDataProvider;
    }

    //public async Task<string> RenderToString()
    //{
    //    using var stringWriter = new StringWriter();

    //    var view = _viewEngine.FindView();

    //    return "";
    //}
}
