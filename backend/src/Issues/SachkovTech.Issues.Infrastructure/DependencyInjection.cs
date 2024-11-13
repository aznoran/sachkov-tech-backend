using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Core.Abstractions;
using SachkovTech.Issues.Application.Interfaces;
using SachkovTech.Issues.Infrastructure.BackgroundServices;
using SachkovTech.Issues.Infrastructure.DbContexts;
using SachkovTech.Issues.Infrastructure.Grpc.Client;
using SachkovTech.Issues.Infrastructure.Repositories;
using SachkovTech.Issues.Infrastructure.Services;

namespace SachkovTech.Issues.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddIssuesInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContexts()
            .AddRepositories()
            .AddDatabase()
            .AddHostedServices()
            .AddServices()
            .AddGrpcNotificationServiceClient(configuration);

        return services;   
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services)
    {
        services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
        services.AddKeyedScoped<IUnitOfWork, UnitOfWork>(SharedKernel.Modules.Issues);

        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;

        return services;
    }

    private static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ILessonsRepository, LessonsRepository>();
        services.AddScoped<IModulesRepository, ModulesRepository>();
        services.AddScoped<IIssueReviewRepository, IssueReviewRepository>();
        services.AddScoped<IUserIssueRepository, UserIssueRepository>();
        
        return services;
    }

    private static IServiceCollection AddDbContexts(this IServiceCollection services)
    {
        services.AddScoped<IssuesWriteDbContext>();
        services.AddScoped<IReadDbContext, IssuesReadDbContext>();

        return services;
    }

    private static IServiceCollection AddHostedServices(this IServiceCollection services)
    {
        //services.AddHostedService<DeleteExpiredIssuesBackgroundService>();

        return services;
    }

    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<DeleteExpiredIssuesService>();

        return services;
    }

    private static IServiceCollection AddGrpcNotificationServiceClient(this IServiceCollection services, IConfiguration configuration)
    {
        var uri = configuration.GetConnectionString(Constants.GRPC_NOTIFICATIONSERVICE_ConnectionStringKey);

        services.AddKeyedSingleton<GrpcChannel>(
            implementationInstance: GrpcChannel.ForAddress(uri!),
            serviceKey: Constants.GRPC_NOTIFICATIONSERVICE_ConnectionStringKey);

        services.AddScoped<IGrpcNotificationServiceClient, GrpcNotificationServiceClient>(sp =>
            {
                GrpcChannel channel = sp.GetKeyedService<GrpcChannel>(Constants.GRPC_NOTIFICATIONSERVICE_ConnectionStringKey)!;
                return new GrpcNotificationServiceClient(channel);
            });

        return services;
    }
}