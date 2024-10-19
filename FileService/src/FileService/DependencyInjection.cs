using FileService.Application.Interfaces;
using FileService.Data.Options;
using FileService.Infrastrucure.Providers;
using Minio;

namespace FileService
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, ConfigurationManager configurations)
        {
            return services;
        }

        public static IServiceCollection AddInfrastrucure(this IServiceCollection services, ConfigurationManager configurations)
        {


            return services;
        }

        private static IServiceCollection AddMinio(
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
    }
}
