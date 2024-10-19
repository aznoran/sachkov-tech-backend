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
    }
}
