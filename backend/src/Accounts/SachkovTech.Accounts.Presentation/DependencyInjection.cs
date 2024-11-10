using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Accounts.Contracts;
using SachkovTech.Accounts.Infrastructure.Providers;
using SachkovTech.Accounts.Presentation.Providers;

namespace SachkovTech.Accounts.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountsPresentation(this IServiceCollection services)
    {
        services.AddScoped<IAccountsContract, AccountsContract>();

        services.AddScoped<HttpContextProvider>();
        services.AddHttpContextAccessor();
        
        return services;
    }
}