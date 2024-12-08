using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Accounts.Contracts;
using SachkovTech.Accounts.Presentation.Providers;

namespace SachkovTech.Accounts.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountsPresentation(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAccountsContract, AccountsContract>();

        services.AddScoped<HttpContextProvider>();
        services.AddHttpContextAccessor();
        
        return services;
    }
}