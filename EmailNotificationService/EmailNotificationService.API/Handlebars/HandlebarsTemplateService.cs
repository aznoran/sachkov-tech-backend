using EmailNotificationService.API.Models;
using HandlebarsDotNet;
using Microsoft.Extensions.Caching.Memory;

namespace EmailNotificationService.API.Handlebars;

public class HandlebarsTemplateService
{
    private const string KEY = "EmailConfirmation";

    private readonly IMemoryCache _cache;

    public HandlebarsTemplateService(IMemoryCache cache)
    {
        _cache = cache;
    }

    public string Process(EmailConfirmationDetails details)
    {
        if (!_cache.TryGetValue(KEY, out HandlebarsTemplate<object, object> compiledTemplate))
        {
            compiledTemplate = HandlebarsDotNet.Handlebars.Compile(
                File.ReadAllText(Path.Combine(
                    Directory.GetCurrentDirectory(), 
                    "wwwroot", 
                    "Templates", 
                    "EmailConfirmation.html")));

            _cache.Set(KEY, compiledTemplate);
        }

        if (compiledTemplate is null)
            throw new ApplicationException("Template error");

        return compiledTemplate(details);
    }

}
