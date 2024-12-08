using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Core.Abstractions;

namespace SachkovTech.Core.Extensions;

public static class MigratorExtensions
{
    public static async Task RunMigrations(this IServiceProvider serviceProvider)
    {
        var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        var migrators = scope.ServiceProvider.GetServices<IMigrator>();
        foreach (var migrator in migrators)
        {
            await migrator.Migrate();
        }
    }
    
    public static async Task RunAutoSeeding(this IServiceProvider serviceProvider)
    {
        var scopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
        using var scope = scopeFactory.CreateScope();
        var seeders = scope.ServiceProvider.GetServices<IAutoSeeder>();
        foreach (var seeder in seeders)
        {
            await seeder.SeedAsync();
        }
    }
}