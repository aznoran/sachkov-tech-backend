using Microsoft.Extensions.DependencyInjection;
using SachkovTech.Core.Abstractions;

namespace SachkovTech.Core.Extensions;

public static class MigratorExtensions
{
    public static void RunMigrations(this IServiceProvider serviceProvider)
    {
        var migrators = serviceProvider.GetServices<IMigrator>();
        foreach (var migrator in migrators)
        {
            migrator.Migrate();
        }
    }
}