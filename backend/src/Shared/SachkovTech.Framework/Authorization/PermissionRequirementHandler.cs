using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Core.Models;
using SachkovTech.Framework.Models;

namespace SachkovTech.Framework.Authorization;

public class PermissionRequirementHandler : AuthorizationHandler<PermissionAttribute>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PermissionRequirementHandler(IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionAttribute permission)
    {
        if (context.User.Identity is null || !context.User.Identity.IsAuthenticated)
        {
            context.Fail();
            return;
        }
        
        var userIdString = context.User.Claims
             .FirstOrDefault(claim => claim.Type == CustomClaims.Id)?.Value;
        
         if (!Guid.TryParse(userIdString, out var userId))
         {
             context.Fail();
             return;
         }

        var permissions = context.User.Claims
            .Where(c => c.Type == CustomClaims.Permission)
            .Select(c => c.Value)
            .ToList();
        
        if (permissions.Contains(permission.Code))
        {
            context.Succeed(permission);
            return;
        }
        
        context.Fail();
    }
}