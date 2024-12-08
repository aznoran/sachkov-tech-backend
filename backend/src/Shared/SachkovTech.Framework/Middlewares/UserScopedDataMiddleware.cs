using System.Security.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using SachkovTech.Core.Models;
using SachkovTech.Framework.Models;

namespace SachkovTech.Framework.Middlewares;

public class UserScopedDataMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<UserScopedDataMiddleware> _logger;

    public UserScopedDataMiddleware(
        RequestDelegate next,
        ILogger<UserScopedDataMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, UserScopedData userScopedData)
    {
        if (context.User.Identity is not null && context.User.Identity.IsAuthenticated)
        {
            var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == CustomClaims.Id)!.Value;

            if (!Guid.TryParse(userIdClaim, out var userId))
                throw new AuthenticationException("The user id claim is not in a valid format.");

            if (userScopedData.UserId == userId)
            {
                await _next(context);
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
        }

        await _next(context);
    }
}

public static class AuthorizationMiddlewareExtensions
{
    public static IApplicationBuilder UseScopeDataMiddleware(
        this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<UserScopedDataMiddleware>();
    }
}