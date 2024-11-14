using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SachkovTech.Core.Models;
using SachkovTech.Framework.Models;

namespace SachkovTech.Framework.Middlewares;

public class AuthorizationMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthorizationMiddleware> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public AuthorizationMiddleware(
        RequestDelegate next,
        ILogger<AuthorizationMiddleware> logger,
        IServiceScopeFactory serviceScopeFactory)
    {
        _next = next;
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            if (context.User.Identity is null || !context.User.Identity.IsAuthenticated)
            {
                await _next(context);
                return;
            }
            
            using var scope = _serviceScopeFactory.CreateScope();

            var userScopedData = scope.ServiceProvider.GetRequiredService<UserScopedData>();
            
            if (userScopedData.UserId != Guid.Empty)
            {
                await _next(context);
                return;
            }
            
            var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == CustomClaims.Id)!.Value;

            if (!Guid.TryParse(userIdClaim, out Guid userId))
            {
                throw new ApplicationException("The user id claim is not in a valid format.");
            }

            userScopedData.UserId = userId;

            userScopedData.Permissions = context.User.Claims
                .Where(c => c.Type == CustomClaims.Permission)
                .Select(c => c.Value)
                .ToList();
            
            userScopedData.Roles = context.User.Claims
                .Where(c => c.Type == CustomClaims.Role)
                .Select(c => c.Value)
                .ToList();
            
            _logger.LogInformation("Roles and permission sets to user scoped data");
            
            context.Items["UserScopedData"] = userScopedData;
            
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
        }
    }
}

public static class AuthorizationMiddlewareExtensions
{
    public static IApplicationBuilder UseAuthorizationMiddleware(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuthorizationMiddleware>();
    }
}