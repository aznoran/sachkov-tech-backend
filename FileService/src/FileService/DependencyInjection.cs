using FileService.Application.Commands.UploadFiles;
using FileService.Application.Interfaces;
using FileService.Application.Queries.GetLinkFiles;
using FileService.Data.Options;
using FileService.Infrastrucure.Abstractions;
using FileService.Infrastrucure.Providers;
using FileService.Infrastrucure.Repositories;
using Minio;
using MongoDB.Driver;

namespace FileService;

public static class DependencyInjection
{
    public static IServiceCollection AddRepositories(
        this IServiceCollection services,
        ConfigurationManager configurations)
    {
        services.AddSingleton(new MongoClient(configurations.GetConnectionString("MongoConnection")));

        services.AddScoped<FileRepository>();

        return services;
    }

    public static IServiceCollection AddMinio(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<MinioOptions>(
            configuration.GetSection(MinioOptions.MINIO));

        services.AddMinio(options =>
        {
            var minioOptions = configuration.GetSection(MinioOptions.MINIO).Get<MinioOptions>()
                               ?? throw new ApplicationException("Missing minio configuration");

            options.WithEndpoint(minioOptions.Endpoint);

            options.WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey);
            options.WithSSL(minioOptions.WithSsl);
        });

        services.AddScoped<IFileProvider, MinioProvider>();

        return services;
    }

    public static IServiceCollection AddCommandsAndQueries(
        this IServiceCollection services)
    {
        //services.Scan(scan => scan.FromAssemblies(typeof(DependencyInjection).Assembly)
        //    .AddClasses(classes => classes
        //        .AssignableToAny(typeof(ICommandHandler<,>), typeof(ICommandHandler<>)))
        //    .AsSelfWithInterfaces()
        //    .WithScopedLifetime());

        //services.Scan(scan => scan.FromAssemblies(typeof(DependencyInjection).Assembly)
        //    .AddClasses(classes => classes
        //        .AssignableToAny(typeof(IQueryHandler<,>), typeof(IQueryHandlerWithResult<,>)))
        //    .AsSelfWithInterfaces()
        //    .WithScopedLifetime());

        services.AddScoped<UploadFilesHandler>();
        services.AddScoped<GetLinkFilesHandler>();

        return services;
    }
}