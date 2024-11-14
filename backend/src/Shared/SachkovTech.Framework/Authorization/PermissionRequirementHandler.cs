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
        //using var scope = _serviceScopeFactory.CreateScope();

        //var userScopedData = scope.ServiceProvider.GetRequiredService<UserScopedData>();

        //временный вариант
        if (context.Resource is HttpContext httpContext &&
            httpContext.Items.TryGetValue("UserScopedData", out var userScopedDataObj) &&
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