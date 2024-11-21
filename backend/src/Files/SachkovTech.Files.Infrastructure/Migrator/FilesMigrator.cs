using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SachkovTech.Core.Abstractions;
using SachkovTech.Files.Infrastructure.Database;

namespace SachkovTech.Files.Infrastructure.Migrator;

public class FilesMigrator(FilesWriteDbContext context, ILogger<FilesMigrator> logger) : IMigrator
{
    public async Task Migrate(CancellationToken cancellationToken = default)
    {
        if (await context.Database.CanConnectAsync(cancellationToken) == false)
        {
            await context.Database.EnsureCreatedAsync(cancellationToken);
        }
        logger.Log(LogLevel.Information, "Applying files migrations...");
        await context.Database.MigrateAsync(cancellationToken);
        logger.Log(LogLevel.Information, "Migrations files applied successfully.");
    }
}