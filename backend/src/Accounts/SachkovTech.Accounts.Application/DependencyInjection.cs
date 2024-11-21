using FileService.Communication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SachkovTech.Accounts.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountsApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddFileHttpCommunication(configuration);

        return services;
    }
}