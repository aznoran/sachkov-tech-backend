using Microsoft.Extensions.DependencyInjection;
using SachkovTech.IssueSolving.Contracts;

namespace SachkovTech.IssueSolving.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddIssueSolvingInfrastructure(
        this IServiceCollection services)
    {
        services.AddScoped<IIssueSolvingContract, IssueSolvingContract>();

        return services;
    }
}