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
}