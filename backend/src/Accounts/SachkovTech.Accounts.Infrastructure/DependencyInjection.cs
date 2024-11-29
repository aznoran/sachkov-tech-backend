using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Accounts.Application;
using SachkovTech.Accounts.Domain;
using SachkovTech.Accounts.Infrastructure.DbContexts;
using SachkovTech.Accounts.Infrastructure.IdentityManagers;
using SachkovTech.Accounts.Infrastructure.Options;
using SachkovTech.Accounts.Infrastructure.Providers;
using SachkovTech.Accounts.Infrastructure.Seeding;
using SachkovTech.Accounts.Infrastructure.TestConsumer;
using SachkovTech.Core.Abstractions;
using SachkovTech.Core.Options;
using SachkovTech.SharedKernel;

namespace SachkovTech.Accounts.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddAccountsInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.RegisterIdentity()
            .AddDbContexts()
            .AddSeeding()
            .ConfigureCustomOptions(configuration)
            .AddMessageBus(configuration)
            .AddProviders();

        return services;
    }

    private static IServiceCollection AddMessageBus(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit<IAccountMessageBus>(configure =>
        {
            configure.SetKebabCaseEndpointNameFormatter();

            configure.AddConsumer<AccountConsumer>();

            configure.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(configuration["RabbitMQ:Host"]!), h =>
                {
                    h.Username(configuration["RabbitMQ:UserName"]!);
                    h.Password(configuration["RabbitMQ:Password"]!);
                });

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }

    private static IServiceCollection RegisterIdentity(this IServiceCollection services)
    {
        services
            .AddIdentity<User, Role>(options => { options.User.RequireUniqueEmail = true; })
            .AddEntityFrameworkStores<AccountsWriteDbContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<PermissionManager>();
        services.AddScoped<RolePermissionManager>();
        services.AddScoped<IAccountsManager, AccountsManager>();
        services.AddScoped<AccountsManager>();
        services.AddScoped<IRefreshSessionManager, RefreshSessionManager>();

        return services;
    }

    private static IServiceCollection AddDbContexts(this IServiceCollection services)
    {
        services.AddScoped<AccountsWriteDbContext>();
        services.AddScoped<IAccountsReadDbContext, AccountsReadDbContext>();

        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(Modules.Accounts);

        return services;
    }

    private static IServiceCollection AddSeeding(this IServiceCollection services)
    {
        services.AddSingleton<AccountsSeeder>();
        services.AddScoped<AccountsSeederService>();

        return services;
    }

    private static IServiceCollection ConfigureCustomOptions(this IServiceCollection services, IConfiguration configuration)
    {

        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.JWT));
        services.Configure<AdminOptions>(configuration.GetSection(AdminOptions.ADMIN));

        return services;
    }

    private static IServiceCollection AddProviders(this IServiceCollection services)
    {
        services.AddTransient<ITokenProvider, JwtTokenProvider>();

        return services;
    }
}