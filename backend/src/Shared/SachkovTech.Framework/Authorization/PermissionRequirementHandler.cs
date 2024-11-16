using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Core.Models;
using SachkovTech.Framework.Models;

namespace SachkovTech.Framework.Authorization;

public class PermissionRequirementHandler : AuthorizationHandler<PermissionAttribute>
{
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public PermissionRequirementHandler(
        IServiceScopeFactory serviceScopeFactory,
        IHttpContextAccessor httpContextAccessor)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionAttribute permission)
    {
        if (context.User.Identity is null || !context.User.Identity.IsAuthenticated 
                                          || _httpContextAccessor.HttpContext is null)
        {
            context.Fail();
            return;
        }

        if (_httpContextAccessor.HttpContext.Items.TryGetValue("user-scoped-data", out var userScopedDataObj) &&
            userScopedDataObj is UserScopedData userScopedData)
        {
            if (userScopedData.Permissions.Contains(permission.Code))
            {
                context.Succeed(permission);
                return;
            }
        }
        
        context.Fail();
    }
}