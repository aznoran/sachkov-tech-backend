using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Core.Abstractions;
using SachkovTech.Issues.Application.Interfaces;
using SachkovTech.Issues.Infrastructure.BackgroundServices;
using SachkovTech.Issues.Infrastructure.DbContexts;
using SachkovTech.Issues.Infrastructure.Repositories;
using SachkovTech.Issues.Infrastructure.Services;

namespace SachkovTech.Issues.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddIssuesInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContexts(configuration)
            .AddRepositories()
            .AddDatabase()
            .AddHostedServices()
            .AddServices();

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
        services.AddScoped<IIssuesReviewRepository, IssuesReviewRepository>();
        services.AddScoped<IUserIssueRepository, UserIssueRepository>();
        services.AddScoped<IModulesRepository, ModulesRepository>();
        services.AddScoped<IIssuesRepository, IssuesesRepository>();

        return services;
    }

    private static IServiceCollection AddDbContexts(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IssuesWriteDbContext>(provider =>
            new IssuesWriteDbContext(configuration.GetConnectionString("Database")!));
        
        services.AddScoped<IReadDbContext, IssuesReadDbContext>(provider =>
            new IssuesReadDbContext(configuration.GetConnectionString("Database")!));

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

    // private static IServiceCollection AddGrpcNotificationServiceClient(this IServiceCollection services,
    //     IConfiguration configuration)
    // {
    //     var uri = configuration.GetConnectionString(Constants.GRPC_NOTIFICATIONSERVICE_ConnectionStringKey);
    //
    //     services.AddKeyedSingleton<GrpcChannel>(
    //         implementationInstance: GrpcChannel.ForAddress(uri!),
    //         serviceKey: Constants.GRPC_NOTIFICATIONSERVICE_ConnectionStringKey);
    //
    //     // services.AddScoped<IGrpcNotificationServiceClient, GrpcNotificationServiceClient>(sp =>
    //     // {
    //     //     GrpcChannel channel =
    //     //         sp.GetKeyedService<GrpcChannel>(Constants.GRPC_NOTIFICATIONSERVICE_ConnectionStringKey)!;
    //     //     return new GrpcNotificationServiceClient(channel);
    //     // });
    //
    //     return services;
    // }
}